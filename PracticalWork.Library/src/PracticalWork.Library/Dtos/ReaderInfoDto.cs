namespace PracticalWork.Library.Dtos;

/// <summary>
/// Информация о читателе
/// </summary>
public class ReaderInfoDto
{
    /// <summary>
    /// Идентификатор карточки читателя
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Фамилия читателя
    /// </summary>
    public string FullName { get; set; }
    
    /// <summary>
    /// Номер телефона читателя
    /// </summary>
    public string PhoneNumber { get; set; }
    
    /// <summary>
    /// Электронная почта читателя
    /// </summary>
    public string Email { get; set; }
}