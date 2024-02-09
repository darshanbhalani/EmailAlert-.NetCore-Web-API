using MailKit.Security;
using MimeKit.Text;
using MimeKit;
using System.Text.Json;
using System.Diagnostics.Metrics;

namespace EmailAlert.Services
{
    public class EmailService : EmailAlertService
    {
        static async Task<string> GetCityCountryAsync(string x, string y)
        {
            string city;
            string country;
            string apiKey = "0081f60753ea47f889423fba2a84a484";
            string apiUrl = $"https://api.opencagedata.com/geocode/v1/json?q={x}+{y}&key={apiKey}";
            await Console.Out.WriteLineAsync(apiUrl);

            using var _httpClient = new HttpClient();
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                var jsonContent = await response.Content.ReadAsStringAsync();
                var jsonDoc = JsonDocument.Parse(jsonContent);
                if (jsonDoc.RootElement.TryGetProperty("results", out JsonElement resultsElement) && resultsElement.GetArrayLength() > 0)
                {
                    var result = resultsElement[0];

                    if (result.TryGetProperty("components", out JsonElement componentsElement))
                    {
                        city = componentsElement.TryGetProperty("city", out var cityElement) ? cityElement.GetString() : componentsElement.GetProperty("county").GetString();
                        country = componentsElement.TryGetProperty("country", out var countryElement) ? countryElement.GetString() : "Country information not available";
                        Console.WriteLine($"City: {city}, Country: {country}");
                    }
                    else
                    {
                        return "City and Country not found!";
                    }
                }
                else
                {
                    return "Failed to fetch data!";
                }

            }
            else
            {
                return $"Failed to fetch data. Status code: {response.StatusCode}";
            }
            return $"{city}, {country}";
        }
        public async void SendEmail(EmailModel request)
        {

            string location = await GetCityCountryAsync(request.X, request.Y);


            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("reilly95@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = "Email Alert";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<!DOCTYPE html>
                        <html>
                        <head>
                            <title>Page Title</title>
                            <link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN' crossorigin='anonymous'>
                            <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js' integrity='sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL' crossorigin='anonymous'></script>
                            <script src='https://kit.fontawesome.com/a076d05399.js' crossorigin='anonymous'></script>
                            <style>
                                .main-box {{
                        max-width:400px;
                                            border: solid 2px black;
                                    display: flex;
                                    justify-content: center;
                                    align-item: center;
                                    flex-direction: column;
                                    padding-top: 10px;
                                    padding-bottom: 10px;
                                }}
                                .btn, .btn:hover {{border: solid 2px blue;
                                    color: white;
                                    background-color: blue;
                                }}
                            </style>
                        </head>
                        <body>
                            <table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""border-collapse:collapse"">
    <tbody>
        <tr>
            <td width=""100%"" align=""center"">
                <table border=""0"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""border-collapse:collapse"">
                    <tbody>
                        <tr>
                            <td width=""1260"" align=""center"">
                                <div style=""max-width:630px;margin:0 auto"" dir=""ltr"" bgcolor=""#ffffff"">
                                    <table border=""0"" cellspacing=""0"" cellpadding=""0"" align=""center""
                                        id=""m_5244377462393205554email_table""
                                        style=""border-collapse:collapse;max-width:630px;margin:0 auto"">
                                        <tbody>
                                            <tr>
                                                <td id=""m_5244377462393205554email_content""
                                                    style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;background:#ffffff"">
                                                    <table border=""0"" width=""100%"" cellspacing=""0"" cellpadding=""0""
                                                        style=""border-collapse:collapse"">
                                                        <tbody>
                                                            <tr>
                                                                <td height=""20"" style=""line-height:20px"" colspan=""3"">
                                                                    &nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td height=""1"" colspan=""3"" style=""line-height:1px""></td>
                                                            </tr>
                                                            <tr>
                                                                <td width=""15"" style=""display:block;width:15px"">
                                                                    &nbsp;&nbsp;&nbsp;</td>
                                                                <td>
                                                                    <table border=""0"" width=""100%"" cellspacing=""0""
                                                                        cellpadding=""0""
                                                                        style=""border-collapse:collapse"">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td height=""20""
                                                                                    style=""line-height:20px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><span
                                                                                        class=""m_5244377462393205554mb_text""
                                                                                        style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;font-size:16px;line-height:21px;color:#141823"">Hi
                                                                                        {request.Name},</span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><span
                                                                                        class=""m_5244377462393205554mb_text""
                                                                                        style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;font-size:16px;line-height:21px;color:#141823"">
                                                                                        {request.Title}
                                                                                    </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><span
                                                                                        class=""m_5244377462393205554mb_text""
                                                                                        style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;font-size:16px;line-height:21px;color:#141823""><strong>About
                                                                                            this change</strong></span>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table border=""0"" width=""100%""
                                                                                        cellspacing=""0"" cellpadding=""0""
                                                                                        style=""border-collapse:collapse"">
                                                                                        <tbody>
                                                                                            <tr style=""height:44px"">
                                                                                                <td><img src=""https://ci3.googleusercontent.com/meips/ADKq_NYY1b_jCu64pG-erdvOvnoyquflxSQjakvhGNy7H4ZXXb4TDvFBYelWqGujLB_tLc-LXvrXi_Ug69Jk9r2RAeOq2BffIjz2cOgyxPDeBaNL_AQ=s0-d-e1-ft#https://static.xx.fbcdn.net/rsrc.php/v3/yD/r/KJyTP_2W_qx.png""
                                                                                                        style=""border:0""
                                                                                                        class=""CToWUd""
                                                                                                        data-bit=""iit"">
                                                                                                </td>
                                                                                                <td>{request.DateTime}</td>
                                                                                            </tr>
                                                                                            <tr
                                                                                                style=""height:44px;vertical-align:center"">
                                                                                                <td><img src=""https://ci3.googleusercontent.com/meips/ADKq_Nb3tsrH0dbDK96bV0ImWOle6zSIX-jE9NIpXEfgT4ZmhvSEAFripnDkpyz5KHyHpx9fdtU3vvDZW1fG5lqqaNmnUxTPc8NuZ8_ACwzcysdG86Y=s0-d-e1-ft#https://static.xx.fbcdn.net/rsrc.php/v3/yK/r/EYwma6hBA2v.png""
                                                                                                        style=""border:0""
                                                                                                        class=""CToWUd""
                                                                                                        data-bit=""iit"">
                                                                                                </td>
                                                                                                <td>{{location}}</td>
                                                                                            </tr>
                                                                                            <tr style=""height:44px"">
                                                                                                <td><img src=""https://ci3.googleusercontent.com/meips/ADKq_NYKoGpf2UDpH7GoIyzjR6vYrqBUszz0vAS3h6TYCeJSrF2Wn4qDK7hnH-I81oCgVaYByUZShGto5ApxNK--zHS1o3mGX692hALVv-Gf_DFaA2c=s0-d-e1-ft#https://static.xx.fbcdn.net/rsrc.php/v3/yC/r/Ve5FEXFHw7A.png""
                                                                                                        style=""border:0""
                                                                                                        class=""CToWUd""
                                                                                                        data-bit=""iit"">
                                                                                                </td>
                                                                                                <td>{request.Device}
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <table border=""0"" width=""100%""
                                                                                        cellspacing=""0"" cellpadding=""0""
                                                                                        style=""border-collapse:collapse"">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td height=""7""
                                                                                                    style=""line-height:7px"">
                                                                                                    &nbsp;</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align=""middle""><a
                                                                                                        href=""https://www.facebook.com/hacked/disavow?u=100010699384613&amp;n=bCcscQuQ&amp;l=en_US&amp;ext=1707145532&amp;hash=AS-pcOX_3lxJKR7OdzE""
                                                                                                        style=""color:#1b74e4;text-decoration:none""
                                                                                                        target=""_blank""
                                                                                                        data-saferedirecturl=""https://www.google.com/url?q=https://www.facebook.com/hacked/disavow?u%3D100010699384613%26n%3DbCcscQuQ%26l%3Den_US%26ext%3D1707145532%26hash%3DAS-pcOX_3lxJKR7OdzE&amp;source=gmail&amp;ust=1707547136588000&amp;usg=AOvVaw0Jv_C9cEJAJAYbE8IKTSku"">
                                                                                                        <table
                                                                                                            border=""0""
                                                                                                            width=""100%""
                                                                                                            cellspacing=""0""
                                                                                                            cellpadding=""0""
                                                                                                            style=""border-collapse:collapse"">
                                                                                                            <tbody>
                                                                                                                <tr>
                                                                                                                    <td
                                                                                                                        style=""border-collapse:collapse;border-radius:6px;text-align:center;display:block;background:#1877f2;padding:8px 20px 8px 20px"">
                                                                                                                        <a href=""https://www.facebook.com/hacked/disavow?u=100010699384613&amp;n=bCcscQuQ&amp;l=en_US&amp;ext=1707145532&amp;hash=AS-pcOX_3lxJKR7OdzE""
                                                                                                                            style=""color:#1b74e4;text-decoration:none;display:block""
                                                                                                                            target=""_blank""
                                                                                                                            data-saferedirecturl=""https://www.google.com/url?q=https://www.facebook.com/hacked/disavow?u%3D100010699384613%26n%3DbCcscQuQ%26l%3Den_US%26ext%3D1707145532%26hash%3DAS-pcOX_3lxJKR7OdzE&amp;source=gmail&amp;ust=1707547136588000&amp;usg=AOvVaw0Jv_C9cEJAJAYbE8IKTSku"">
                                                                                                                            <center>
                                                                                                                                <font
                                                                                                                                    size=""3"">
                                                                                                                                    <span
                                                                                                                                        style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;white-space:nowrap;font-weight:bold;vertical-align:middle;color:#ffffff;font-weight:500;font-family:Roboto-Medium,Roboto,-apple-system,BlinkMacSystemFont,Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;font-size:17px"">This&nbsp;wasn't&nbsp;me</span>
                                                                                                                                </font>
                                                                                                                            </center>
                                                                                                                        </a>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </a></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td height=""8""
                                                                                                    style=""line-height:8px"">
                                                                                                    &nbsp;</td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td height=""13""
                                                                                                    style=""line-height:13px"">
                                                                                                    &nbsp;</td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><span
                                                                                        class=""m_5244377462393205554mb_text""
                                                                                        style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;font-size:16px;line-height:21px;color:#141823"">If
                                                                                        this was you, you don't need to
                                                                                        do anything.</span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td><span
                                                                                        class=""m_5244377462393205554mb_text""
                                                                                        style=""font-family:Helvetica Neue,Helvetica,Lucida Grande,tahoma,verdana,arial,sans-serif;font-size:16px;line-height:21px;color:#141823"">Thanks,<br>
                                                                                        The Security Team</span></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td height=""15""
                                                                                    style=""line-height:15px"">&nbsp;</td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                                <td width=""15"" style=""display:block;width:15px"">
                                                                    &nbsp;&nbsp;&nbsp;</td>
                                                            </tr>
                                                        </tbody>
                                                    </table><span><img
                                                            src=""https://ci3.googleusercontent.com/meips/ADKq_Na9rfg7WokFWzX5WBa0kic61EfNyq3smWZ1Spw9O0F5ywfexkWUuGSCV6HVMfRY8NRE16y47wVa-8dWjW-0bK3bJHU5-pm7QFfKesRWCQfO8xJ5V6EvudvIgNxR73xUONkpX0S2zErFh67x8jPuDbjOx_SWb1o=s0-d-e1-ft#https://www.facebook.com/email_open_log_pic.php?mid=61016b08b9435G5af58e35e725G61016fa219707G38d""
                                                            style=""border:0;width:1px;height:1px"" class=""CToWUd""
                                                            data-bit=""iit""></span>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
        </tr>
    </tbody>
</table>
                        </body>
                        </html>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("reilly95@ethereal.email", "ktFvu9fuzN97ceguwU");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

    }
}
