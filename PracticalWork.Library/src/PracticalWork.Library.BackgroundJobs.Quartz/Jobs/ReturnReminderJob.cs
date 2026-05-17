using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PracticalWork.Library.Abstractions.Services.Infrastructure;
using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Dtos;
using PracticalWork.Library.Options.Email;
using Quartz;

namespace PracticalWork.Library.BackgroundJobs.Quartz.Jobs;

/// <summary>
/// Фоновая задача для автоматического напоминания читателей о возврате книг
/// </summary>
public class ReturnReminderJob : BaseJob
{
    private readonly ILogger<ReturnReminderJob> _logger;
    private readonly IBookBorrowRepository _bookBorrowRepository;
    private readonly IEmailService _emailService;
    private readonly ReturnReminderTemplate _returnReminderTemplate;

    public ReturnReminderJob(
        ILogger<ReturnReminderJob> logger,
        IEmailService emailService,
        IBookBorrowRepository bookBorrowRepository,
        IOptions<EmailTemplateSettings> emailTemplateSettings)
    {
        _logger = logger;
        _emailService = emailService;
        _bookBorrowRepository = bookBorrowRepository;
        _returnReminderTemplate = emailTemplateSettings.Value.ReturnReminder;
    }

    /// <inheritdoc cref="BaseJob.JobName"/>
    public override string JobName { get; } = "Return Reminder Job";

    /// <inheritdoc cref="BaseJob.Description"/>
    public override string Description { get; } = "Задача для автоматического напоминания читателям о возврате книг.";

    /// <inheritdoc cref="BaseJob.ExecuteJob"/>
    protected override async Task ExecuteJob(IJobExecutionContext context, CancellationToken cancellationToken)
    {
        var activeBorrows = await
            _bookBorrowRepository.GetBorrowsDueInDays(_returnReminderTemplate.DaysBeforeDueDate, cancellationToken);
        
        var success = 0;
        var failed = 0;

        foreach (var borrow in activeBorrows)
        {
            var readerInfo = await _bookBorrowRepository.GetReaderInfoByBorrowedBookId(borrow.BookId, cancellationToken);
            var messageSubject = string.Format(_returnReminderTemplate.SubjectTemplate, borrow.Title);
            
            try
            {
                var message = new ReturnReminderEmailMessageDto
                {
                    ReaderFullName = readerInfo.FullName,
                    BookTitle = borrow.Title,
                    BookAuthors = string.Join(" ", borrow.Authors),
                    ReturnDate = DateOnly
                        .FromDateTime(DateTime.UtcNow)
                        .AddDays(_returnReminderTemplate.DaysBeforeDueDate),
                    DaysLeft = _returnReminderTemplate.DaysBeforeDueDate,
                    LibraryAddress = _returnReminderTemplate.LibraryAddress,
                    LibraryPhone = _returnReminderTemplate.LibraryPhone,
                    WorkingHours = _returnReminderTemplate.WorkingHours,
                };

                await _emailService.SendAsync(readerInfo.Email, messageSubject, message, cancellationToken);

                _logger.LogInformation(
                    "Письмо успешно отправлено на почту {email}. Книга для возврата - {bookId}",
                    readerInfo.Email,
                    borrow.BookId);

                success++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Не удалось отправить письмо на почту {email} для возврата книги с ID={bookId}",
                    readerInfo.Email,
                    borrow.BookId);

                failed++;
            }
        }

        _logger.LogInformation(
            "Отправка напоминаний о возврате книг завершена!" +
            "Писем отправлено: {success}, Не удалось отправить: {failed}",
            success, failed);
    }
}