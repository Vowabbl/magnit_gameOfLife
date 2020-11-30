--Задача №1
WITH temp AS (   
  SELECT  
    ArtID,
    CntrID,
    DayID,
    EndQnty,
    GroupingSet = DATEADD(DAY,-ROW_NUMBER() OVER(PARTITION BY ArtID,CntrID ORDER BY DayID), DayID)
  FROM tTestTable1
)

SELECT
  ArtID AS 'Товар',
  CntrID AS 'Контрагент',        
  'Количество' = Sum(EndQnty),
  'Кол-во дней без отгрузок' = COUNT(NULLIF(ArtID, 0)),
  'Начальная дата' = MIN(DayID),
  'Конечная дата' = MAX(DayID) 
FROM temp
GROUP BY ArtID, CntrID, GroupingSet
HAVING COUNT(NULLIF(ArtID, 0)) > 1
ORDER BY ArtID, CntrID;
--------------------------------------------------------------------
--Задача №3

WITH temp AS (
  SELECT 
    'Неделя ' + cast(datepart(wk, DayID) AS varchar(2)) Week,
    year(DayID) Год,
    SUM(CASE WHEN IncomingType = 1 OR IncomingType = 2 THEN Volume ELSE 0 END) 'sum incomeType1-2',
    SUM(CASE WHEN SignID = 1 OR IncomingType = 1 THEN Volume ELSE 0 END) 'sum incomeType,incomeSign 1'
  FROM tTestTable2
  GROUP BY  datepart(wk, DayID), year(DayID)
)

SELECT 
  week AS 'Неделя', 
  ([sum incomeType,incomeSign 1]/[sum incomeType1-2])*100 AS 'Доля' 
FROM 
  temp
--------------------------------------------------------------------------------------------------------------
--Задача №4

WITH temp AS(
  SELECT 
      'Неделя ' + cast(datepart(wk, DayID) as varchar(2)) week,
      year(DayID) year,
      sum(Price) as price,
      sum(PriceCalc) as priceEst,
      SignID as 'incomeSign',
      IncomingType as 'incomeType',
      ArtID
  FROM tTestTable2  
  group by  datepart(wk, DayID), year(DayID), SignID, IncomingType, ArtID
)

SELECT week, price AS 'Доля объема', priceEst - price AS 'Разница' FROM temp
  WHERE priceEst < price AND incomeSign = 1 AND incomeType = 1

