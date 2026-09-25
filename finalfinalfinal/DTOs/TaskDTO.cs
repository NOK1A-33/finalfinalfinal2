using finalfinalfinal.Models; 


namespace finalfinalfinal.DTOs
{
    public record TaskCreateDto(
        string Title,
        string? Description,
        DateTime? DueDate,
        Priority Priority,
        List<string>? Tags,
        int? ProjectId);

    public record TaskUpdateDto(
        string? Title,
        string? Description,
        DateTime? DueDate,
        Priority? Priority,
        bool? Completed,
        List<string>? Tags,
        int? ProjectId);
}


    