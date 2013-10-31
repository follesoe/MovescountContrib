namespace MovescountContrib.Lib

open System
open Newtonsoft.Json
open System.Text.RegularExpressions
open MovescountContrib.Lib.DataTypes

module PlannedMoves =
    open MovescountContrib.Lib.MovescountClient
    open MovescountContrib.Lib.Utils
    open DDay.iCal

    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then Some(List.tail [ for g in m.Groups -> g.Value ])
        else None

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

    let getStartTime (move:ScheduledMove) =
        match move.Description with
        | Regex @"(\d{2}):(\d{2})" [hourStr; minuteStr] ->
            let hours = Convert.ToDouble(hourStr)
            let minutes = Convert.ToDouble(minuteStr)
            Some( (move.Day.Date.AddHours(hours).AddMinutes(minutes)) )
        | _ -> None

    let getTrainingPlanCal email password =
        let plan = (getTrainingPlan email password) |> Async.RunSynchronously
        let timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time")

        let iCal = new iCalendar()
        let calName = sprintf "%s (Movecount)" email
        let calDescription = sprintf "Movescount.com Planned Moves for %s" email

        iCal.AddProperty("X-WR-CALNAME", calName)
        iCal.AddProperty("X-WR-CALDESC", calDescription)
        iCal.AddTimeZone(timeZone) |> ignore

        plan.ScheduledMoves |>
            Seq.iter(fun m ->

                let event = iCal.Create<Event>()
                event.Properties.Remove("SEQUENCE") |> ignore

                match getStartTime m with
                | Some(date) ->
                    event.IsAllDay <- false
                    event.Start <- new iCalDateTime(date)
                    event.End <- new iCalDateTime((date.AddMinutes((Convert.ToDouble(m.Duration)))))
                | None ->
                    event.IsAllDay <- true
                    event.Start <- new iCalDateTime(m.Day.Date)

                event.Description <- m.Description
                event.Summary <- getEventTitle m
                event.UID <- (m.ID.ToString())
                ())
            |> ignore
        iCal