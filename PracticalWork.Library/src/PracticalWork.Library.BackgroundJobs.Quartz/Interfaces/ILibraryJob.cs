using Quartz;

namespace PracticalWork.Library.BackgroundJobs.Quartz.Interfaces;

/// <summary>
/// Интерфейс для всех фоновых задач в системе управления библиотекой
/// </summary>
public interface ILibraryJob : IJob
{
    /// <summary>
    /// Уникальное имя задачи
    /// </summary>
    string JobName { get; }
    
    /// <summary>
    /// Описание задачи для отображения в интерфейсе управления
    /// </summary>
    string Description { get; }
}