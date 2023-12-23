namespace Domain.Models;

public record RepairRequestStatus(int Id, string DisplayName) : Enumeration<RepairRequestStatus>(Id, DisplayName)
{
    public static readonly RepairRequestStatus New = new(1, "Новий");
    public static readonly RepairRequestStatus Accepted = new(2, "Прийнятий на розгляд");
    public static readonly RepairRequestStatus InProgress = new(3, "В обробці");
    public static readonly RepairRequestStatus Completed = new(4, "Завершено");
    public static readonly RepairRequestStatus Cancelled = new(5, "Скасовано");
    public static readonly RepairRequestStatus Rejected = new(6, "Відхилено");
    public static readonly RepairRequestStatus Archived = new(7, "Архівовано");
}