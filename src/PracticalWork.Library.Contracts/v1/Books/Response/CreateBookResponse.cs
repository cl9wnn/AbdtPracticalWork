﻿namespace PracticalWork.Library.Contracts.v1.Books.Response;

/// <summary>
/// Ответ на создание книги
/// </summary>
/// <param name="Id">Идентификатор книги</param>
public sealed record CreateBookResponse(Guid Id);