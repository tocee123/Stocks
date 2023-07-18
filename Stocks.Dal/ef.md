## To drop 
```sql
drop table StockDividend;
drop table StockPrice;
drop table Stock;
drop table stockOfInterest;
drop table __EFMigrationsHistory;
```

## To remove
```sql
truncate table StockDividend;
truncate table StockPrice;
delete from Stock;
```

## To select
```sql
select * From stock s
left join stockdividend sd on s.id = sd.stockId
left join stockprice sp on s.id = sp.stockId
where s.ticker = 'arr'
```
```
select * From stockOfInterest;
```

```sql
drop table StockDividend;
drop table StockPrice;
drop table Stock;
drop table stockOfInterest;
drop table __EFMigrationsHistory;

select * From stock s
left join stockdividend sd on s.id = sd.stockId
cross apply (select top 1 sp.price from stockprice sp where sp.stockid = s.id order by sp.date desc) sp
where s.ticker = 'arr'

select * from stockprice;

select * From stockOfInterest;

truncate table StockDividend;
truncate table StockPrice;
delete from Stock ;

delete from StockDividend where stockId = 180
```
