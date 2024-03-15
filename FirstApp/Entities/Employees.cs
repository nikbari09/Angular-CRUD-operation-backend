using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace FirstApp.Entities
{
    public class Employees
    {
        public int id { get; set; }
        public string firstname { get; set; }

        public string lastname { get; set; }

        public string email { get; set; }

        public DateOnly dob { get; set; }
        public string gender { get; set; }
    }


public static class EmployeesEndpoints
{
	public static void MapEmployeesEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Employees").WithTags(nameof(Employees));

        group.MapGet("/", () =>
        {
            return new [] { new Employees() };
        })
        .WithName("GetAllEmployees")
        .WithOpenApi();

        group.MapGet("/{id}", (int id) =>
        {
            //return new Employees { ID = id };
        })
        .WithName("GetEmployeesById")
        .WithOpenApi();

        group.MapPut("/{id}", (int id, Employees input) =>
        {
            return TypedResults.NoContent();
        })
        .WithName("UpdateEmployees")
        .WithOpenApi();

        group.MapPost("/", (Employees model) =>
        {
            //return TypedResults.Created($"/api/Employees/{model.ID}", model);
        })
        .WithName("CreateEmployees")
        .WithOpenApi();

        group.MapDelete("/{id}", (int id) =>
        {
            //return TypedResults.Ok(new Employees { ID = id });
        })
        .WithName("DeleteEmployees")
        .WithOpenApi();
    }
}}
