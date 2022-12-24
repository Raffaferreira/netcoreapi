using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Presentation.MinimalApi
{
    public static class CreditRouting
    {
        public static void RegisterMinimalApiDebit(this WebApplication app)
        {
            app.MapGet("/credit", async (WebApiDbContext db) =>
            {
                return await db.Credito.ToListAsync();
            });

            app.MapGet("/credit/{accountId}", async (Guid accountId, WebApiDbContext db) =>
            {
                return await db.Credito.FindAsync(accountId)
                is Credito dbt ?
                Results.Ok(dbt) : Results.NotFound();
            });

            app.MapPost("/credit", async (Credito crdt, WebApiDbContext db) =>
            {
                db.Credito.Add(crdt);
                await db.SaveChangesAsync();

                return Results.Created($"/credit/{crdt.Id}", crdt);
            });

            app.MapPut("/credit/{accountId}", async (Guid accountId, Credito crdt, WebApiDbContext db) =>
            {
                var credito = await db.Credito.FindAsync(accountId);

                if (credito is null) return Results.NotFound();

                credito.AccountTobeCredited = crdt.AccountTobeCredited;
                credito.Value = crdt.Value;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapMethods("/credit/{accountId}", new string[] { "PATCH" }, async (Guid accountId, Credito crdt, WebApiDbContext db) =>
            {
                var credito = await db.Credito.FindAsync(accountId);

                if (credito is null) return Results.NotFound();

                credito.AccountTobeCredited = crdt.AccountTobeCredited;
                credito.Value = crdt.Value;

                await db.SaveChangesAsync();

                return Results.NoContent();
            });

            app.MapDelete("/credit/{accountId}", async (Guid accountId, WebApiDbContext db) =>
            {
                if (await db.Credito.FindAsync(accountId) is Credito credito)
                {
                    db.Credito.Remove(credito);
                    await db.SaveChangesAsync();
                    return Results.Ok(credito);
                }

                return Results.NotFound();
            });
        }
    }
}
