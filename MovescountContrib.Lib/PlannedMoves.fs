﻿namespace MovescountContrib.Lib

open Newtonsoft.Json
open System.Text.RegularExpressions
open MovescountContrib.Lib.DataTypes

module PlannedMoves =
    open MovescountContrib.Lib.MovescountClient
    open MovescountContrib.Lib.Utils
    open DDay.iCal

    let getTrainingPlan email password =
        async {
            let! content = (getContent email password "http://www.movescount.com/plannedmoves")
            let jsonString =
                Regex.Matches(content, "initialScheduledTrainingProgramAsJSON = \"(.*)\"")
                |> Seq.cast<Match>
                |> Seq.last
                |> fun m -> m.Groups.[1].Value
                |> fun j -> j.Replace("\\\"", "\"").Replace("\\\/", "/")

            let settings = new JsonSerializerSettings()
            settings.StringEscapeHandling <- StringEscapeHandling.EscapeHtml
            return JsonConvert.DeserializeObject<TrainingPlan>(jsonString, settings)
        }

    let getTrainingPlanCal email password =
        let plan = (getTrainingPlan email password) |> Async.RunSynchronously
        let iCal = new iCalendar()
        let calName = sprintf "%s (Movecount)" email
        let calDescription = sprintf "Movescount.com Planned Moves for %s" email

        iCal.AddProperty("X-WR-CALNAME", calName)
        iCal.AddProperty("X-WR-CALDESC", calDescription)

        plan.ScheduledMoves |>
            Seq.iter(fun m ->
                let event = iCal.Create<Event>()
                event.Properties.Remove("SEQUENCE") |> ignore
                event.IsAllDay <- true
                event.Start <- new iCalDateTime(m.Day.Date)
                event.Description <- m.Description
                event.Summary <- getEventTitle m
                event.UID <- (m.ID.ToString())
                ())
            |> ignore
        iCal