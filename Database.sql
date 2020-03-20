use master;

create database EconoMiC;

use EconoMiC;

create table Users
(
ID int identity(0,1),
LOGIN nvarchar(32) unique not null,
PASSWORD nvarchar(32) unique not null,
TOTAL_MONEY float not null,
LAST_ACTIVITY date
constraint FK_Users primary key (ID)
)

alter table Users add LAST_ACTIVITY date;

create table Targets
(
USER_ID int,
TARGET_NAME nvarchar(32) not null,
TOTAL_SUM float not null,
CURRENT_SUM float default(0) not null,
SPENDS float,
DEADLINE_DATA date
constraint
FK_Targets_Users foreign key (USER_ID)
references Users(ID)
)

create table Categories
(
USER_ID int,
CATEGORY_NAME nvarchar(32) not null,
SPENDS float,
constraint
FK_Categories_Users foreign key (USER_ID)
references Users(ID)
)


create table Incomes
(
USER_ID int,
INCOME_NAME nvarchar(32) not null,
INCOME_MONEY float not null,
month smallint not null,
day smallint not null
constraint
FK_Incomes_Users foreign key (USER_ID)
references Users(ID)
)

insert into Users (LOGIN,PASSWORD,TOTAL_MONEY)
values ('Danil','Danil',10)

select top(1) * from Users where LOGIN='Danil' and PASSWORD='Danil'

