namespace MovescountContrib.Lib

open System
open System.IO
open System.Net
open System.Web
open System.Runtime.Serialization
open System.Text.RegularExpressions

module MovescountClient =
    let private getAuthToken email password =
        let emailEncoded = HttpUtility.UrlEncode(email, Text.Encoding.UTF8)
        let url = sprintf "https://servicegate.suunto.com/UserAuthorityService/?callback=jQuery19106261484450660646_1383085432762&service=Movescount&emailAddress=%s&password=%s&_=1383085432763" emailEncoded password
        async {
            let req = HttpWebRequest.Create(url) :?> HttpWebRequest
            let! res = req.AsyncGetResponse()
            use stream = res.GetResponseStream()
            use reader = new StreamReader(stream)
            let contents = reader.ReadToEnd()
            let token = Regex.Matches(contents, "(\"(.*?)\")")
                        |> Seq.cast<Match>
                        |> Seq.head
                        |> fun m-> m.Value.Replace("\"", "")
            return token
        }

    let private setAuthCookie authToken (cookies:CookieContainer) =
        async {
            let req = HttpWebRequest.Create("http://www.movescount.com/SuuntoPassLogin.aspx/UserAuthenticated") :?> HttpWebRequest
            req.Method <- "POST"
            req.CookieContainer <- cookies
            req.ContentType <- "application/json";

            let payload = sprintf "{\"token\":\"%s\",\"utcOffset\":\"60\",\"redirectUri\":\"/scoreboard\"}" authToken
            use reqStream = req.GetRequestStream()
            use writer = new StreamWriter(reqStream)
            writer.WriteLine(payload)
            writer.Flush()

            let! res = req.AsyncGetResponse()
            ()
        }

    let getContent email password (url:string) =
        async {
            let! authToken = getAuthToken email password
            let cookies = new CookieContainer(100)
            let! cookieTask = setAuthCookie authToken cookies

            let req = HttpWebRequest.Create(url) :?> HttpWebRequest
            req.CookieContainer <- cookies

            let! res = req.AsyncGetResponse()
            let stream = res.GetResponseStream()
            let reader = new StreamReader(stream)
            let contents = reader.ReadToEnd()
            return contents
        }