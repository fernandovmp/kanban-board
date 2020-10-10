using FluentMigrator.Builders;
using FluentMigrator.Builders.Create.Table;

namespace KanbanBoard.Migrations
{
    public static class FluentMigratorExtensions
    {

        public static ICreateTableColumnOptionOrWithColumnSyntax AsSerial(this IColumnTypeSyntax<ICreateTableColumnOptionOrWithColumnSyntax> columnTypeSyntax)
            => columnTypeSyntax.AsCustom("serial");

        public static ICreateTableColumnOptionOrWithColumnSyntax AsTimestamp2(this IColumnTypeSyntax<ICreateTableColumnOptionOrWithColumnSyntax> columnTypeSyntax)
            => columnTypeSyntax.AsCustom("timestamp(2)");

        public static ICreateTableColumnOptionOrWithColumnSyntax AsText(this IColumnTypeSyntax<ICreateTableColumnOptionOrWithColumnSyntax> columnTypeSyntax)
            => columnTypeSyntax.AsCustom("text");
    }
}
