﻿namespace MovescountContrib.Lib

open MovescountContrib.Lib.DataTypes

module Utils =
    let getIntensityName intensityLevel =
        match intensityLevel with
        | 1 -> "Easy"
        | 2 -> "Moderate"
        | 3 -> "Hard"
        | 4 -> "Very Hard"
        | 5 -> "Maximal"
        | _ -> raise (System.Exception(sprintf "%d is an invalid intensity level" intensityLevel))

    let getActivityName activityId =
        match activityId with
        | 1 -> "Not specified sport"
        | 2 -> "Multisport"
        | 3 -> "Run"
        | 4 -> "Cycling"
        | 5 -> "Mountain Biking"
        | 6 -> "Swimming"
        | 8 -> "Skating"
        | 9 -> "Aerobics"
        | 10 -> "Yoga/Pilates"
        | 11 -> "Trekking"
        | 12 -> "Walking"
        | 13 -> "Sailing"
        | 14 -> "Kayaking"
        | 15 -> "Rowing"
        | 16 -> "Climbing"
        | 17 -> "Indoor cycling"
        | 18 -> "Circuit training"
        | 19 -> "Triathlon"
        | 20 -> "Alpine skiing"
        | 21 -> "Snowboarding"
        | 22 -> "Crosscountry skiing"
        | 23 -> "Weight training"
        | 24 -> "Basketball"
        | 25 -> "Soccer"
        | 26 -> "Ice Hockey"
        | 27 -> "Volleyball"
        | 28 -> "Football"
        | 29 -> "Softball"
        | 30 -> "Cheerleading"
        | 31 -> "Baseball"
        | 33 -> "Tennis"
        | 34 -> "Badminton"
        | 35 -> "Table tennis"
        | 36 -> "Racquet ball"
        | 37 -> "Squash"
        | 38 -> "Combat sport"
        | 39 -> "Boxing"
        | 40 -> "Floorball"
        | 51 -> "Scuba diving"
        | 52 -> "Free diving"
        | 61 -> "Adventure Racing"
        | 62 -> "Bowling"
        | 63 -> "Cricket"
        | 64 -> "Cross trainer"
        | 65 -> "Dancing"
        | 66 -> "Golf"
        | 67 -> "Gymnastics"
        | 68 -> "Handball"
        | 69 -> "Horseback riding"
        | 70 -> "Ice Skating"
        | 71 -> "Indoor Rowing"
        | 72 -> "Canoeing"
        | 73 -> "Motorsports"
        | 74 -> "Mountaineering"
        | 75 -> "Orienteering"
        | 76 -> "Rugby"
        | 78 -> "Ski Touring"
        | 79 -> "Stretching"
        | 80 -> "Telemark skiing"
        | 81 -> "Track and Field"
        | 82 -> "Trail Running"
        | 83 -> "Open water swimming"
        | 84 -> "Nordic walking"
        | 85 -> "Snow shoeing"
        | 86 -> "Windsurfing/Surfing"
        | 87 -> "Kettlebell"
        | 88 -> "Roller skiing"
        | 89 -> "Standup paddling (SUP)"
        | 90 -> "Cross fit"
        | 91 -> "Kitesurfing/Kiting"
        | 92 -> "Paragliding"
        | 93 -> "Treadmill"
        | _ -> raise (new System.Exception((sprintf "%d is not a valid activity ID" activityId)))

    let getEventTitle (plannedMove:ScheduledMove) =
        let activityName = getActivityName plannedMove.ActivityID
        let intensityName = getIntensityName plannedMove.IntensityLevel
        sprintf "%s for %d min - %s" activityName plannedMove.Duration intensityName