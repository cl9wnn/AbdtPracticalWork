﻿using PracticalWork.Library.Abstractions.Storage;
using PracticalWork.Library.Enums;

namespace PracticalWork.Library.Data.PostgreSql.Entities;

/// <summary>
/// Выдачи книги на руки читателю
/// </summary>
public sealed class BookBorrowEntity : EntityBase
{
    /// <summary>Идентификатор книги</summary>
    public Guid BookId { get; set; }

    /// <summary>Идентификатор карточки читателя</summary>
    public Guid ReaderId { get; set; }

    /// <summary>Дата выдачи книги</summary>
    public DateOnly BorrowDate { get; set; }

    /// <summary>Срок возврата книги</summary>
    public DateOnly DueDate { get; set; }

    /// <summary>Фактическая дата возврата книги</summary>
    public DateOnly ReturnDate { get; set; }

    /// <summary>Статус книги в библиотеке</summary>
    public BookIssueStatus Status { get; set; }
}