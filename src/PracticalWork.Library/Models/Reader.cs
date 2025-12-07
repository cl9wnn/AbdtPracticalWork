using PracticalWork.Library.Exceptions;

namespace PracticalWork.Library.Models;

/// <summary>
/// Карточка читателя
/// </summary>
public sealed class Reader
{
    /// <summary> ФИО</summary>
    /// <remarks> Запись идет через пробел</remarks>
    public string FullName { get; set; }
    
    /// <summary> Номер телефона</summary>
    public string PhoneNumber { get; set; }
    
    /// <summary> Дата окончания действия карточки</summary>
    public DateOnly ExpiryDate { get; set; }
    
    /// <summary> Активность карточки</summary>
    public bool IsActive { get; set; }
    
    /// <summary>Проверка валидности карточки</summary>
    public bool IsValid() => IsActive && DateOnly.FromDateTime(DateTime.UtcNow) <= ExpiryDate;

    /// <summary>
    /// Продление срока действия карточки
    /// </summary>
    /// <param name="newExpiryDate">Новая дата окончания действия карточки</param>
    public void Extend(DateOnly newExpiryDate)
    {
        if (!IsValid())
        {
            throw new ReaderServiceException("Карточка не действительна!");
        }

        if (newExpiryDate < ExpiryDate)
        {
            throw new ReaderServiceException("Новая дата окончания не может быть устаревшей!");
        }
        
        ExpiryDate = newExpiryDate;
    }

    /// <summary>Закрытие карточки читателя</summary>
    public void Close()
    {
        if (!IsValid())
        {
            throw new ReaderServiceException("Карточка не действительна!");
        }
        
        IsActive = false;
        ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }
}