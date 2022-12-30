using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Presentation.MinimalApi
{
    public static class DebitRouting 
    {
        public static void RegisterMinimalApiCredit(this WebApplication app)
        {
            app.MapGet("/debits", async (WebApiDbContext db) =>
            {
                return await db.Debito.ToListAsync();
            });

            app.MapGet("/debit/{accoutNumber}", async (Guid Id, WebApiDbContext db) =>
            {
                return await db.Debito.FindAsync(Id)
                is Debito transactions ?
                Results.Ok(transactions) : Results.NotFound();
            });

            app.MapPost("/debit", async (Debito debit, WebApiDbContext db) =>
            {
                db.Debito.Add(debit);
                await db.SaveChangesAsync();

                return Results.Created($"/transactions/{debit.Id}", debit);
            });

            app.MapPut("/debit/{accountId}", async (Guid accountId, Debito dbt, WebApiDbContext db) =>
            {
                var todo = await db.Debito.FindAsync(accountId);

                if (todo is null) return Results.NotFound();

                todo.Id = dbt.Id;
                todo.AccountTobeWithdraw = dbt.AccountTobeWithdraw;

                await db.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapDelete("/debit/{accountId}", async (Guid accountId, WebApiDbContext db) =>
            {
                if (await db.Debito.FindAsync(accountId) is Debito dbt)
                {
                    db.Debito.Remove(dbt);
                    await db.SaveChangesAsync();
                    return Results.Ok(dbt);
                }

                return Results.Ok();
            });
        }
    }
}
