namespace Minimal.API.Models;

/// <summary> DTO for an ingredient.</summary>
public class IngredientDto
{
    /// <summary>The unique identifier for the ingredient.</summary>
    public Guid Id { get; set; }
    
    /// <summary>The name of the ingredient.</summary>
    public required string Name { get; set; }
    
    /// <summary>The unique identifier for the dish that the ingredient belongs to.</summary>
    public Guid DishId { get; set; }
}