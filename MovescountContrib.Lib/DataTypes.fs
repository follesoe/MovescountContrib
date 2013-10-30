namespace MovescountContrib.Lib.DataTypes

open System
open System.Runtime.Serialization

[<DataContract>]
type ScheduledMove = {
    [<field: DataMember(Name = "ID")>]
    ID : int
    [<field: DataMember(Name = "ActivityID")>]
    ActivityID : int
    [<field: DataMember(Name = "Day")>]
    Day : DateTime
    [<field: DataMember(Name = "Ordinal")>]
    Ordinal : int
    [<field: DataMember(Name = "Duration")>]
    Duration : int
    [<field: DataMember(Name = "IntensityLevel")>]
    IntensityLevel : int
    [<field: DataMember(Name = "Description")>]
    Description : string
}

[<DataContract>]
type TrainingPlan = {
    [<field: DataMember(Name = "ID")>]
    ID : int
    [<field: DataMember(Name = "ScheduledMoves")>]
    ScheduledMoves : System.Collections.Generic.List<ScheduledMove>
}