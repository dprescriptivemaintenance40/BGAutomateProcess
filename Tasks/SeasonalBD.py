from typing import final
import pandas as pd
import matplotlib.pyplot as plt
from statsmodels.tsa.seasonal import seasonal_decompose
from statsmodels.tsa.ar_model import AutoReg
import numpy as np
import sys
import pyodbc as odbc

bigdata = pd.DataFrame()
# Step 1 - Read the CSV file
conn = odbc.connect('Driver=SQL SERVER;Server=DESKTOP-CGG65T8;Database=DPM;Integrated Security=True')

cursor = conn.cursor()
batchId=int(sys.argv[1])
query='''SELECT * FROM Compressor_Processed WHERE BatchId={}'''
series = pd.read_sql(query.format(batchId),conn)

series.set_index('Date')
# Step 3 :- 
series.index=pd.to_datetime(series.index, unit='D',origin=min(series['Date']))
# Step 4 :- Season decomponse it in to observer , seasonal , trend
for col in series.columns:
    print("Col name" + col)
    if not (col=="Date" or "Unnamed" in col):
        result=seasonal_decompose(series[col], model='additive', period=365,extrapolate_trend='freq')
        
        resultFinal = pd.concat([result.observed, result.seasonal,result.trend,result.resid],axis=1)
        resultFinal.index.name  = col+"Date"
        # Step 5 :- Added the totalunknow=seasonal + residual
        resultFinal[col+'resses'] =result.seasonal+result.resid
        resultFinal[col+'datemonth']  = resultFinal.index.strftime("%m/%d")
        datemonth = pd.DataFrame(columns = [col+'datemonth',col+'average'])
        datemonth[col+'datemonth'] = resultFinal[col+'datemonth']
        # Step 6 :0 datemonth is having date and month with averag
        dtmean=resultFinal.groupby([col+'datemonth'])[col+'resses'].mean()
        #print(dtmean)
        #plt.show()

        Training = resultFinal[col].copy()
        model = AutoReg(Training, lags=3)
        model_fit = model.fit()
        predictedValue = model_fit.predict(len(resultFinal), len(resultFinal)+800)

        #predictedValue["finalvalue"]=0
        #predictedValue['datemonth']  = predictedValue.iloc[:, [0]].strftime("%m/%d")
        panda1 = pd.DataFrame({col+'Date':predictedValue.index,col+ 'ValuePredicted':predictedValue.values})
        panda1[col+"datemonth"]= panda1[col+"Date"].dt.strftime("%m/%d")

        finalDf = pd.merge(pd.DataFrame(panda1), pd.DataFrame(dtmean), left_on=[col+'datemonth'], 
                    right_on= [col+'datemonth'], how='left')
        # value predited from autoregression plus unknows
        finalDf[col+"New"] = finalDf[col+"ValuePredicted"] + finalDf[col+"resses"]
       
        finalDf.set_index(col+'Date', inplace=True)
        bigdata[col+" Preicted"]=finalDf[col+"New"]
    
bigdata.to_csv("BU3Final.csv")