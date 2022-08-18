
import sys
import pandas as pd
import pyodbc as odbc


conn = odbc.connect('Driver=SQL SERVER;Server=DESKTOP-CGG65T8;Database=DPM;Integrated Security=True')

cursor = conn.cursor()
batchId=int(sys.argv[1])
query='''SELECT * FROM Compressor_Cleaning WHERE BatchId={}'''
MissVal = pd.read_sql(query.format(batchId),conn, parse_dates=['Date'])
MissVal.drop_duplicates(subset=['Date'],inplace=True)
  # Step 3 :- Generatte dates from min and max values
date_range = pd.DataFrame({'Date': pd.date_range(min(MissVal['Date']),max(MissVal['Date']), freq='D')})
  # Step 4 :- Make left join with Missing values.
c = pd.merge(pd.DataFrame(date_range), pd.DataFrame(MissVal), left_on=['Date'], 
              right_on= ['Date'], how='left')

for col in c.columns:
    if not col=="Date":
      if not col=="Id":
        if not col=="BatchId":
          c[col].interpolate(method ='linear',inplace=True)
c.to_csv("DbFilledData1.csv")
for i, row in c.iterrows():
  if pd.isnull(row['BatchId']):
    row['BatchId']=batch
  else:
    row['BatchId']=int(row['BatchId'])
    batch=int(row['BatchId'])
  SQLCommand = "INSERT INTO Compressor_Processed(BatchId,Date,TD1,TS1,TD2,TS2,PD1,PD2,DT1,DT2,PR1,PR2) VALUES('" + str(row['BatchId']) + "','" + str(row['Date']) + "','" + str(row['TD1']) + "','" + str(row['TS1']) + "','" + str(row['TD2']) + "','" + str(row['TS2']) + "','" + str(row['PD1']) + "','" + str(row['PD2'])+ "','" + str(row['DT1']) + "','" + str(row['DT2']) + "','" + str(row['PR1']) + "','" + str(row['PR2']) + "')"
  cursor.execute(SQLCommand)

conn.commit()
          #c.to_sql("Compressor_Cleaning",conn)
# Step 5:- Create a new column which will be interpolated
# for col in c.columns:
#     if not col=="Date":
#       if not col=="Id":
#         if not col=="BatchId":
#           #c[col+"new"] = c[col]
#           c[col].interpolate(method ='linear',inplace=True)
#           SQLCommand = ("INSERT INTO Compressor_Processed (BatchId,Date,TD1,TS1,TD2,TS2,PD1,PD2,DT1,DT2,PR1,PR2) VALUES (?,?,?,?,?,?,?,?,?,?,?,?)")    
#           Values = [0, c.Date, c.TD1, c.TS1, c.TD2, c.TD2, c.PD1, c.PD2, c.DT1, c.DT2, c.PR1, c.PR2]   
#           #Processing Query    
#           cursor.execute(SQLCommand,Values)  
          #cursor.execute('INSERT INTO Compressor_Processed (BatchId,TD1,TS1,TD2,TS2,PD1,PD2,DT1,DT2,PR1,PR2) values(?,?,?,?,?,?,?,?,?,?,?)', c.BatchId, c.TD1, c.TS1, c.TD2, c.TD2, c.PD1, c.PD2, c.DT1, c.DT2, c.PR1, c.PR2)
          #c.to_csv("DbFilledData.csv")