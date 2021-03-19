use CarDealer

select * from Dealer.Banks
select * from Prog.GetCarInfo('Arkana I')

insert into Dealer.Banks values ('Nona')

select * from Dealer.Credits
update Dealer.Credits
set expiration = 18
where id = 3

select * from Prog.GetCarsByName('D')