using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Presentation.MinimalApi
{
    public static class TransactionsRouting
    {
        public static void RegisterMinimalApiTransaction(this WebApplication app)
        {
            app.MapGet("/transactions", async (WebApiDbContext db) =>
            {
                return await db.Transactions.ToListAsync();
            });

            app.MapGet("/transactions/{accountId}", async (Guid accountId, WebApiDbContext db) =>
            {
                return await db.Transactions.FindAsync(accountId)
                is Transactions transactions ?
                Results.Ok(transactions) : Results.NotFound();
            });

            app.MapPost("/transactions", async (Transactions transaction, WebApiDbContext db) =>
            {
                db.Transactions.Add(transaction);
                await db.SaveChangesAsync();

                return Results.Created($"/transactions/{transaction.Id}", transaction);
            });

            app.MapPut("/transactions/{accountId}", async (Guid accountId, Transactions trs, WebApiDbContext db) =>
            {
                var todo = await db.Transactions.FindAsync(accountId);

                if (todo is null) return Results.NotFound();

                todo.AccountNumber = trs.AccountNumber;
                todo.Balance = trs.Balance;

                await db.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapDelete("/transactions/{id}", async (Guid accountId, WebApiDbContext db) =>
            {
                if (await db.Transactions.FindAsync(accountId) is Transactions todo)
                {
                    db.Transactions.Remove(todo);
                    await db.SaveChangesAsync();
                    return Results.Ok(todo);
                }

                return Results.NotFound();
            });
        }
    }
}
