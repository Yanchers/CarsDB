use CarDealer

drop schema Dealer

go
create schema Dealer
go
create schema Prog

alter schema Prog transfer Dealer.GetPercents

exec Dealer.AddBank 'Jopa'

select * from Dealer.Banks

drop table Dealer.CarsCredits
drop table Dealer.Credits
drop table Dealer.CarsFactories
drop table Dealer.Cars
drop table Dealer.Factories
drop table Dealer.Banks

create table Dealer.Factories
(
	id int identity primary key,
	country nvarchar(50),
	city nvarchar(50),
	transpCost money,
	deliveryTime int,
	[type] int
)
create table Dealer.Cars
(
	id int identity primary key,
	[name] nvarchar(30) not null unique,
	cost money
)
create table Dealer.CarsFactories
(
	carId int references Dealer.Cars(id) on delete cascade not null primary key clustered(carId asc, factoryId asc),
	factoryId int references Dealer.Factories(id) on delete cascade not null
)
create table Dealer.Banks
(
	id int identity primary key,
	[name] nvarchar(50) not null unique
)
create table Dealer.Credits
(
	id int identity primary key,
	bankId int references Dealer.Banks(id) on delete cascade not null,
	rate float not null,
	expiration int not null
)
create table Dealer.CarsCredits
(
	carId int references Dealer.Cars(id) on delete cascade not null primary key clustered(carId asc, creditId asc),
	creditId int references Dealer.Credits(id) on delete cascade not null
)

insert into Dealer.Factories values
('Russia', 'Saint-Petersburg', 20000, 7, 3),
('Ukraine', 'Ismail', 16000, 3, 1),
('Belarus', 'Gomel', 11000, 6, 4),
('Russia', 'Moscow', 26000, 4, 2)

insert into Dealer.Factories values ('Brazil', 'Tropico', 29300, 2, 3)
select * from Dealer.Factories
delete from Dealer.Factories where id = 5

insert into Dealer.Cars values
('Clio V', 600000),
('Arkana I', 1081000),
('Duster II', 760000),
('Kaptur I Рестайлинг', 1400000),
('Logan I', 209000),
('Symbol II', 227000)

insert into Dealer.CarsFactories values
(4,4),
(6,3),
(5,1),
(3,2),
(4,1),
(5,4),
(2,3),
(1,2)
select * from Dealer.CarsFactories
insert into Dealer.Banks values
('ВТБ'),
('Сбербанк'),
('СельХозБанк'),
('Тинькофф')

insert into Dealer.Credits values
(3, 3.2, 18),
(2, 3.4, 20),
(4, 2.7, 15),
(2, 3.4, 12),
(1, 2.8, 13)

insert into Dealer.CarsCredits values
(5, 5),
(3, 4),
(1, 4),
(2, 3),
(1, 2),
(4, 2),
(6, 1),
(6, 5),
(4, 3)

select * from Dealer.CarsCredits
select * from Dealer.CarsFactories
delete top(1) from Dealer.CarsCredits
where carId = 6 and creditId = 5

-- Функция, возвращающая ежемесячный платёж
drop function Dealer.GetMonthlyPay
go
create function Dealer.GetMonthlyPay
(@cost int, @months int, @koef float)
returns float
as begin
	declare @monthly float = (@koef/100) / 12
	return @cost * (@monthly * power((1 + @monthly), @months) / (power((1 + @monthly), @months) - 1))
end
go

-- Функция, возвращающая сумму процентов
drop function Dealer.GetPercents
go
create function Dealer.GetPercents
(@cost int, @months int, @koef float)
returns float
as begin
	declare @monthly float = (@koef / 100) / 12
	declare @monthPay float = @cost * (@monthly * power((1 + @monthly), @months) / (power((1 + @monthly), @months) - 1))
	declare @percents float = @cost * @monthly
	
	while @months != 0
	begin
		set @cost -= @monthPay - @cost * @monthly
		set @percents += @cost * @monthly
		set @months -= 1
	end

	return @percents
end
go

select Dealer.GetPercents(300000, 18, 2.4)

select Dealer.GetMonthlyPay(300000, 18, 2.4)

-- Функция, возвращающая таблицу с информацией о машине
drop function Prog.GetCarInfo
go
create function Prog.GetCarInfo
(@model nvarchar(50))
returns @ret table
(
	creditId int,
	factoryId int,
	carCost money,
	totalCost money,
	monthlyPay money, 
	expiration date, 
	bankName nvarchar(50),
	country nvarchar(50),
	city nvarchar(50),
	transpCost money,
	arrival date
)
as begin
	declare @carId int = (select id from Dealer.Cars where [name] = @model)
	declare @creditId int, @months int, @percent float, @bankId int
	declare creditCursor cursor local
	for 
	select id, expiration, rate, bankId from Dealer.Credits
	join Dealer.CarsCredits on CarsCredits.creditId = Credits.id
	where carId = @carId

	open creditCursor

	fetch next from creditCursor into @creditId, @months, @percent, @bankId
	while @@FETCH_STATUS = 0
	begin
		declare @cost money = (select cost from Dealer.Cars where id = @carId)
		declare @bankName nvarchar(50) = (select [name] from Dealer.Banks where id = @bankId)
		declare @factoryId int, @country nvarchar(50), @city nvarchar(50), @arrival int, @transpCost money
		declare factoriesCursor cursor local
		for
		select id, country, city, deliveryTime, transpCost from Dealer.Factories
		join Dealer.CarsFactories on factoryId = id
		where carId = @carId

		open factoriesCursor
		fetch next from factoriesCursor into @factoryId, @country, @city, @arrival, @transpCost
		while @@FETCH_STATUS = 0
		begin
			insert into @ret values
			(@creditId, @factoryId, @cost,
			round(@cost + Prog.GetPercents(@cost, @months, @percent) + @transpCost, 2),
			round(Prog.GetMonthlyPay(@cost, @months, @percent), 2),
			dateadd(month, @months, GETDATE()),
			@bankName, @country, @city, @transpCost, dateadd(day, @arrival, GETDATE()))

			fetch next from factoriesCursor into @factoryId, @country, @city, @arrival, @transpCost
		end
		close factoriesCursor
		deallocate factoriesCursor

		fetch next from creditCursor into @creditId, @months, @percent, @bankId
	end

	close creditCursor
	deallocate creditCursor
	return
end
go
select * from Prog.GetCarInfo('Logan I')

-- Функция, возвращающая авто, созданные указанным заводом
drop function Dealer.GetCarsByFactory
go
create function Dealer.GetCarsByFactory(@factoryId int)
returns table
as return
	select Cars.* from Dealer.Cars
	join Dealer.CarsFactories on carId = id
	where factoryId = @factoryId
go
select * from Dealer.GetCarsByFactory(3)

drop function Dealer.GetCarsByName
go -- Функция, возвращающая авто с указанным названием модели
create function Dealer.GetCarsByName(@name nvarchar(50))
returns table
as return
	select * from Dealer.Cars
	where [name] like '%'+@name+'%'
go

drop function Dealer.GetCarsByPrice
go -- Функция, возвращающая авто, у которых цена в диапазоне между @upper и @down
create function Dealer.GetCarsByPrice(@upper money, @down money)
returns table
as return
	select * from Dealer.Cars
	where cost <= @upper and cost >= @down
go

-- =================================================================

drop procedure Dealer.AddCar
go -- Процедура, добавляющая Авто и связывающая его с заводом и кредитом
create procedure Dealer.AddCar
(@name nvarchar(50), @cost money)
as begin
	begin try
		begin transaction
			insert into Dealer.Cars values (@name, @cost)
		commit transaction
	end try
	begin catch
		rollback transaction
		raiserror('Модель с таким названием уже существует.', 14, 1)
	end catch
end
go
exec Dealer.AddCar 'Tess', -890000
delete from Dealer.Cars
where [name] = 'Tess'

drop procedure Dealer.AddFactory
go -- Процедура, добавляющая Завод
create procedure Dealer.AddFactory
(@country nvarchar(50), @city nvarchar(50), @deliveryTime int, @type int, @transpCost money)
as begin
	begin try
		begin transaction
	insert into Dealer.Factories values (@country, @city, @transpCost, @deliveryTime, @type)
		commit transaction
	end try
	begin catch
		rollback transaction
		raiserror('Ошибка при добавлении завода.', 14, 1)
	end catch
end
go

drop procedure Dealer.AddCarAndFactory
go -- Процедура, добавляющая Завод и Авто, связывающая их вместе и кредитом
create procedure Dealer.AddCarAndFactory
(@name nvarchar(50), @cost money, @creditId int,
@country nvarchar(50), @city nvarchar(50), @deliveryTime int, @type int, @transpCost money)
as begin
	insert into Dealer.Cars values
	(@name, @cost)
	declare @carId int = @@IDENTITY
	insert into Dealer.CarsCredits values
	(@carId, @creditId)

	insert into Dealer.Factories values
	(@country, @city, @transpCost, @deliveryTime, @type)
	insert into Dealer.CarsFactories values
	(@carId, @@IDENTITY)
end
go

drop procedure Dealer.AddBank
go
create procedure Dealer.AddBank
(@name nvarchar(50))
as begin
	begin try
		begin transaction
			insert into Dealer.Banks values (@name);
		commit transaction
	end try
	begin catch
		rollback transaction
		raiserror('Название банка должно быть уникальным.', 14, 1)
	end catch
end
go
select * from Dealer.Banks

drop procedure Dealer.AddCredit
go
create procedure Dealer.AddCredit
(@bankId int, @rate float, @expiration int)
as begin
	begin try
		begin transaction
			insert into Dealer.Credits values
			(@bankId, @rate, @expiration)
		commit transaction
	end try
	begin catch
		rollback transaction
		raiserror('ID выбранного банка не существует.', 14, 1)
	end catch
end
go

drop procedure Dealer.AddCarCreditFactoryRef
go
create procedure Dealer.AddCarCreditFactoryRef
(@carId int, @creditId int, @factoryId int)
as begin
	begin try
		begin transaction
			if not exists(select * from Dealer.CarsCredits where carId = @carId and creditId = @creditId)
				insert into Dealer.CarsCredits values (@carId, @creditId)
			if not exists(select * from Dealer.CarsFactories where carId = @carId and factoryId = @factoryId)
				insert into Dealer.CarsFactories values (@carId, @factoryId)
		commit transaction
	end try
	begin catch
		rollback transaction
	end catch
end
go
exec Dealer.AddCarCreditFactoryRef 2, 2, 2




-- ===========================================================
drop trigger Dealer.OnCarInsert
go
create trigger Dealer.OnCarInsert
on Dealer.Cars
instead of insert
as begin
	insert into Dealer.Cars
	select [name], cost from inserted
	where [name] not in (select [name] from Dealer.Cars)
end
go

insert into Dealer.Cars values
('Arkana II', 890900)
select * from Dealer.Cars

drop trigger Dealer.OnFactoryDelete
go
create trigger Dealer.OnFactoryDelete
on Dealer.Factories
instead of delete
as begin
	if not exists(select * from Dealer.Factories
					join deleted on Factories.id = deleted.id)
		raiserror('Завода(-ов) с указанным id не существует.', 14, 1)
	else
		delete from Dealer.Factories
		where id in (select id from deleted)
end

drop trigger Dealer.OnBankDelete
go
create trigger Dealer.OnBankDelete
on Dealer.Banks
instead of delete
as begin
	if not exists(select * from Dealer.Banks
					join deleted on Banks.id = deleted.id)
		raiserror('Указанных банков не существует.', 14, 1)
	else
		delete from Dealer.Banks
		where id in (select id from deleted)
end
select * from Dealer.Banks
delete from Dealer.Banks
where [name] = 'ХДДД'

--go -- Наверное не надо ======================== !!!
--create trigger Dealer.OnCreditDelete
--on Dealer.Credits
--instead of delete
--as begin
--	if not exists(select * from Dealer.Credits
--					join deleted on Credits.id = deleted.id)
--		raiserror('Указанных кредитов не существует.', 14, 1)
--	else
--		delete from Dealer.Credits
--		where id in (select id from deleted)
--end

--go 
--create trigger OnCredditAdd
--on Dealer.Credits
--instead of insert
--as begin
--	if exists(select id from Dealer.Credits
--				where rate in (select rate from deleted)
--				and expiration in (select expiration from deleted))
--		raiserror('Кредит с такими')
--end
use CarDealer
drop user Administrator
create user Administrator for login [Admin]
grant select, insert, update, delete, execute on schema :: Dealer to Administrator