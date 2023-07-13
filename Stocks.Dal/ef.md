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
