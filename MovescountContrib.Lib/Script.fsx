#r "System.Net"
#r "System.Web"
#r "System.Runtime"
#r "System.Runtime.Serialization"
#r @"..\packages\Newtonsoft.Json.5.0.8\lib\net40\Newtonsoft.Json.dll"
#r @"..\packages\DDay.iCal.1.0.2.575\lib\DDay.iCal.dll"

#load "DataTypes.fs"
#load "MovescountClient.fs"
#load "PlannedMoves.fs"

open MovescountContrib.Lib

//let plan = (PlannedMoves.getTrainingPlan "jonas@follesoe.no" "password") |> Async.RunSynchronously
let cal = (PlannedMoves.getTrainingPlanCal "jonas@follesoe.no" "password") |> Async.RunSynchronously