
import sys
import pandas as pd
import pyodbc as odbc


conn = odbc.connect('Driver=SQL SERVER;Server=DESKTOP-CGG65T8;Database=DPM;Integrated Security=True')

cursor = conn.cursor()
batchId=int(sys.argv[1])
AssetName=str(sys.argv[2])
# batchId=2
# AssetName="CentrifugalCompressor"

#ScrewParamter
if AssetName == "ScrewCompressor" :
    query='''SELECT * FROM ScrewCleaningTables WHERE SPId={}'''
    MissVal = pd.read_sql(query.format(batchId),conn, parse_dates=['Date'])
    # MissVal.drop_duplicates(subset=['Date'],inplace=True)
      # Step 3 :- Generatte dates from min and max values
    date_range = pd.DataFrame({'Date': pd.date_range(min(MissVal['Date'])
                    ,max(MissVal['Date']), freq='D')})
      # Step 4 :- Make left join with Missing values.
    c = pd.merge(pd.DataFrame(date_range), pd.DataFrame(MissVal), 
              left_on=['Date'],
              right_on= ['Date'], how='left')
    for col in c.columns:
        if not col=="Date":
          if not col=="Id":
            if not col=="SPId":
              c[col].interpolate(method ='linear',inplace=True)
    c.to_csv("DbFilledScrew.csv")
    for i, row in c.iterrows():
      if pd.isnull(row['SPId']):
        row['SPId']=batch
      else:
        row['SPId']=int(row['SPId'])
        batch=int(row['SPId'])
      SQLCommand = "INSERT INTO ScrewProcessedTables(SPId,Date,TD1,TD2,DT1,DT2,PR1,PR2) VALUES('" + str(row['SPId']) + "','" + str(row['Date']) + "','" + str(row['TD1']) + "','" + str(row['TD2']) + "','" + str(row['DT1']) + "','" + str(row['DT2']) + "','" + str(row['PR1']) + "','" + str(row['PR2']) + "')"
      cursor.execute(SQLCommand)
    conn.commit()

#CentrifugalParamter
elif AssetName == "CentrifugalCompressor" or AssetName == "CentrifugalPump" :
    query='''SELECT * FROM CentrifugalCleaningTables WHERE CPId={}'''
    MissVal = pd.read_sql(query.format(batchId),conn, parse_dates=['Date'])
    #MissVal.drop_duplicates(subset=['Date'],inplace=True)
      # Step 3 :- Generatte dates from min and max values
    date_range = pd.DataFrame({'Date': pd.date_range(min(MissVal['Date'])
                 ,max(MissVal['Date']), freq='D')})
      # Step 4 :- Make left join with Missing values.
    c = pd.merge(pd.DataFrame(date_range), pd.DataFrame(MissVal), 
              left_on=['Date'], 
              right_on= ['Date'], how='left')
    for col in c.columns:
        if not col=="Date":
          if not col=="Id":
            if not col=="CPId":
              c[col].interpolate(method ='linear',inplace=True)
    c.to_csv("DbFilledCentrifugal.csv")
    for i, row in c.iterrows():
      if pd.isnull(row['CPId']):
        row['CPId']=batch
      else:
        row['CPId']=int(row['CPId'])
        batch=int(row['CPId'])
      SQLCommand = "INSERT INTO CentrifugalProcessedTables(CPId,Date,Vibration3H) VALUES('" + str(row['CPId']) + "','" + str(row['Date']) + "','" + str(row['Vibration3H']) + "')"
      cursor.execute(SQLCommand)
    conn.commit()

#ReciprocatingParamter
elif AssetName == "ReciprocatingCompressor" or AssetName == "ReciprocatingPump" or AssetName == "RotaryPump" :
    query='''SELECT * FROM ReciprocatingCleaningTables WHERE RPId={}'''
    MissVal = pd.read_sql(query.format(batchId),conn, parse_dates=['Date'])
    # MissVal.drop_duplicates(subset=['Date'],inplace=True)
      # Step 3 :- Generatte dates from min and max values
    date_range = pd.DataFrame({'Date': pd.date_range(min(MissVal['Date'])
                  ,max(MissVal['Date']), freq='D')})
      # Step 4 :- Make left join with Missing values.
    c = pd.merge(pd.DataFrame(date_range), pd.DataFrame(MissVal), 
              left_on=['Date'], 
              right_on= ['Date'], how='left')
    for col in c.columns:
        if not col=="Date":
          if not col=="Id":
            if not col=="RPId":
              c[col].interpolate(method ='linear',inplace=True)
    c.to_csv("DbFilledReciprocating.csv")
    for i, row in c.iterrows():
      if pd.isnull(row['RPId']):
        row['RPId']=batch
      else:
        row['RPId']=int(row['RPId'])
        batch=int(row['RPId'])
      SQLCommand = "INSERT INTO ReciprocatingProcessedTables(RPId,Date,TDValve) VALUES('" + str(row['RPId']) + "','" + str(row['Date']) + "','" + str(row['TDValve']) + "')"
      cursor.execute(SQLCommand)
    conn.commit()