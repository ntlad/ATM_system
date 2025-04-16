using FluentMigrator;
using Itmo.Dev.Platform.Postgres.Migrations;

namespace Lab5.Infrastucture.DataAccess.Migrations;

[Migration(1, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
     """
     create type user_role as enum
     (
         'admin',
         'customer'
     );

     create type transaction_type as enum
     (
         'deposit', 
         'withdraw', 
         'check_balance', 
         'create', 
         'get_history'
     );

     create type transaction_result_state as enum
     (
         'completed',
         'rejected'
     );

     create table users
     (
         user_id bigint primary key generated always as identity ,
         user_name text not null ,
         user_role user_role not null ,
         password text not null ,
         balance bigint not null
     );

     create table transactions
     (
         transaction_id bigint primary key generated always as identity ,
         user_id bigint not null ,
         transaction_type text not null
     );
     """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
    """
    drop table users;
    drop table transactions;

    drop type user_role;
    drop type transaction_type;
    drop type transaction_result_state;
    """;
}