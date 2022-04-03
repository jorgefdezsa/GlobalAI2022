// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ManagementPageController.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
// </copyright>
// <summary>
//   JoinCallController is a third-party controller (non-Bot Framework) that can be called in CVI scenario to trigger the bot to join a call
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ViejadelVisilloBot.Services.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Swashbuckle.AspNetCore.Annotations;
    using System.Net.Http;
    using System.Text;
    using ViejadelVisilloBot.Model.Constants;

    /// <summary>
    /// Management page allowing joining, listing, and leaving calls.
    /// </summary>
    public class ManagementPageController : ControllerBase
    {
        /// <summary>
        /// The join call async.
        /// </summary>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [SwaggerOperation(
        Summary = "Management",
            Description = "",
            Tags = new[] { "Management" }
        )]
        [SwaggerResponse(StatusCodes.Status200OK, "Management Successfully", typeof(ContentResult))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid input data", typeof(ValidationProblemDetails))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal Server Error", null)]
        [HttpGet]
        [Route(HttpRouteConstants.Management + "/")]
        public ContentResult ManagementPage()
        {
            //var html = @"<!DOCTYPE html>" +
            //    "<html lang='en'>" +
            //    "<head>" +
            //    "<meta charset='UTF-8'><title>Teams Bot</title>" +
            //    "<style>" +
            //    "html { scroll-behavior: smooth; }" +
            //    "* { box-sizing: border-box; } " +
            //    "body { background-color: #040714; color: #f9f9f9; font-family: Avenir-Roman, sans-serif; margin: 0; padding: 0; }" +
            //    "a { color: #f9f9f9; text-decoration: none; }" +
            //    "@media only screen and (min-width: 768px) {  body { font-size: 16px; } }" +
            //    "@media only screen and (min-width: 480px) and (max-width: 768px) { body { font-size: 15px; }}" +
            //    "@media only screen and (max-width: 479px) { body { font-size: 14px; }}" +
            //    ".qfjcY { height: 70px; background: #090b13; display: flex; -webkit-box-align: center; align-items: center; padding: 0 36px; }" +
            //    ".bYDabe { position: relative; min-height: calc(100vh - 250px); overflow-x: hidden; display: block; padding: 0 calc(3.5vw + 5px);}" +
            //    ".WmJmX { width: 80px; }" +
            //    ".klbOuJ a img { height: 20px; }" +
            //    ".iqtRwK button { z-index: 1;}" +
            //    ".klbOuJ { display: flex; flex: 1; margin-left: 20px; -webkit-box-align: center; align-items: center; }" +
            //    ".klbOuJ a { display: flex; -webkit-box-align: center; align-items: center; padding: 0 12px; cursor: pointer; }" +
            //    ".dxwKfM { width: 48px; height: 48px; border-radius: 50%; cursor: pointer; }" +
            //    ".cDeQOc { margin-top: 20px; }" +
            //    ".slick-dots { list-style: none; }" +
            //    ".slick-prev, .slick-next { font-size: 0; line-height: 0; position:absolute; top: 50%; width: 20px; height: 20px; padding: 0; transform: translate(0, -50%); cursor: pointer; color: transparent; border: none; outline: none; background: transparent; }" +
            //    ".slick-dots li{ position: relative; display: inline-block; width: 20px; height: 20px; margin: 0 5px; padding: 0; cursor:po}" +
            //    ".slick-slider { position: relative; display: block; box-sizing: border-box; -webkit-user-select: none; user-select: none; -webkit-touch-callout: none; -khtml-user-select: none; touch-action: pan-y; -webkit-tap-highlight-color: transparent; }" +
            //    ".slick-list { position: relative; display: block; overflow: hidden; margin: 0; padding: 0; }" +
            //    ".slick-list:focus { outline: none; } " +
            //    ".slick-list.dragging { cursor: pointer; cursor: hand; } " +
            //    ".slick-slider .slick-track, .slick-slider .slick-list { transform: translate3d(0, 0, 0); }" +
            //    ".slick-track { position: relative; top: 0; left: 0; display: block; margin-left: auto; margin-right: auto; } " +
            //    ".slick-track:before, .slick-track:after { display: table; content: ''; }" +
            //    ".slick-track:after { clear: both; } " +
            //    ".slick-loading .slick-track { visibility: hidden; }" +
            //    ".slick-slide { display: none; float: left; height: 100%; min-height: 1px; }" +
            //    ".slick-dots li button:before { font-family: 'slick'; font-size: 6px; line-height: 20px; position: absolute;
            //top: 0;
            //left: 0;

            //width: 20px;
            //height: 20px;

            //content: '•';
            //    text - align: center;

            //opacity: .25;
            //color: black;

            //    -webkit - font - smoothing: antialiased;
            //    -moz - osx - font - smoothing: grayscale;
            //}
            //"
            //    ".xXyTL {  margin-top: 30px; display: grid; padding: 30px 0px 26px; grid-gap: 25px; grid-template-columns: repeat(3, minmax(0, 1fr)); }" +
            //    ".dWazIg { border-radius: 10px; border: 3px solid rgba(249, 249, 249, 0.1); box-shadow: rgb(0 0 0 / 69 %) 0px 26px 30px - 10px, rgb(0 0 0 / 73 %) 0px 16px 10px - 10px; transition: all 250ms cubic-bezier(0.25, 0.46, 0.45, 0.94) 0s; }" +
            //    "</style>" +
            //    "</head>" +
            //    "<body>" +
            //    "<div class='App'><nav class='sc-bdvvtL qfjcY'><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/parrita_logo.png' class='sc-gsDKAQ WmJmX'><div class='sc-dkPtRN klbOuJ'><a><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/home-icon.svg' alt=''><span>HOME</span></a><a><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/search-icon.svg' alt=''><span>SEARCH</span></a><a><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/watchlist-icon.svg' alt=''><span>WATCHLIST</span></a><a><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/original-icon.svg' alt=''><span>ORIGINALS</span></a><a><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/movie-icon.svg' alt=''><span>MOVIES</span></a><a><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/series-icon.svg' alt=''><span>SERIES</span></a></div><img src='https://yt3.ggpht.com/yti/APfAmoGU2phakWR8ro_JSVAdcG6cTpFOeON_hxsEYVOgV_0=s88-c-k-c0x00ffffff-no-rj-mo' alt='' class='sc-hKwDye dxwKfM'></nav><main class='sc-bkkeKt bYDabe'><div class='slick-slider sc-eCImPb cDeQOc slick-initialized' dir='ltr'><button type='button' data-role='none' class='slick-arrow slick-prev' style='display: block;'> Previous</button><div class='slick-list'><div class='slick-track' style='width: 1810px; opacity: 1; transform: translate3d(-724px, 0px, 0px); transition: -webkit-transform 500ms ease 0s;'><div data-index='-1' tabindex='-1' class='slick-slide slick-cloned' aria-hidden='true' style='width: 362px;'><div><div tabindex='-1' class='sc-jRQBWg jhASnR' style='width: 100%; display: inline-block;'><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/slider-badag.png' alt=''></div></div></div><div data-index='0' class='slick-slide' tabindex='-1' aria-hidden='true' style='outline: none; width: 362px;'><div><div tabindex='-1' class='sc-jRQBWg jhASnR' style='width: 100%; display: inline-block;'><img src='https://storageaccountrdglobbd7.blob.core.windows.net/imgs/slider-scales.png' alt=''></div></div></div><div data-index='1' class='slick-slide slick-active slick-current' tabindex='-1' aria-hidden='false' style='outline: none; width: 362px;'><div><div tabindex='-1' class='sc-jRQBWg jhASnR' style='width: 100%; display: inline-block;'><img src='/images/slider-badag.png' alt=''></div></div></div><div data-index='2' tabindex='-1' class='slick-slide slick-cloned' aria-hidden='true' style='width: 362px;'><div><div tabindex='-1' class='sc-jRQBWg jhASnR' style='width: 100%; display: inline-block;'><img src='/images/slider-scales.png' alt=''></div></div></div><div data-index='3' tabindex='-1' class='slick-slide slick-cloned' aria-hidden='true' style='width: 362px;'><div><div tabindex='-1' class='sc-jRQBWg jhASnR' style='width: 100%; display: inline-block;'><img src='/images/slider-badag.png' alt=''></div></div></div></div></div><button type='button' data-role='none' class='slick-arrow slick-next' style='display: block;'> Next</button><ul class='slick-dots' style='display: block;'><li class=''><button>1</button></li><li class='slick-active'><button>2</button></li></ul></div><div class='sc-fotOHu xXyTL'><div class='sc-fFeiMQ dWazIg'><p>Marcelo</p></div><div class='sc-fFeiMQ dWazIg'><p>ponte</p></div><div class='sc-fFeiMQ dWazIg'><p>a</p></div><div class='sc-fFeiMQ dWazIg'><p>trabajar</p></div><div class='sc-fFeiMQ dWazIg'><p>de</p></div><div class='sc-fFeiMQ dWazIg'><p>una</p></div><div class='sc-fFeiMQ dWazIg'><p>puta</p></div><div class='sc-fFeiMQ dWazIg'><p>vez</p></div><div class='sc-fFeiMQ dWazIg'><p>maricona</p></div></div><div class='sc-iqseJM LXykG'><h4>Recommended for You</h4><div class='sc-crHmcD eVEPhn'></div></div><div class='sc-gKclnd kioMDe'><h4>New to Disney+</h4><div class='sc-iCfMLu hkCyuY'></div></div><div class='sc-pVTFL hlFESP'><h4>Originals</h4><div class='sc-jrQzAO cpAsRl'></div></div><div class='sc-bqiRlB hKujbJ'><h4>Trending</h4><div class='sc-ksdxgE hbpwGC'></div></div></main></div>" +
            //    "</body>" +
            //    "</html>";



            //var html =
            //    @"<!DOCTYPE html>
            //      <html lang='en'>
            //          <head>
            //              <meta charset='UTF-8'>
            //              <title>Teams Bot</title>
            //              <style>
            //                  body {
            //                        background-color: #040714;
            //                        color: #f9f9f9;
            //                        font-family: Avenir-Roman, sans-serif;
            //                        margin: 0;
            //                        padding: 0;
            //                  }
            //                a {
            //                  color: #f9f9f9;
            //                  text-decoration: none;
            //                }

            //                @media only screen and (min-width: 768px) {
            //                  body {
            //                    font-size: 16px;
            //                  }
            //                }
            //                @media only screen and (min-width: 480px) and (max-width: 768px) {
            //                  body {
            //                    font-size: 15px;
            //                  }
            //                }
            //                @media only screen and (max-width: 479px) {
            //                  body {
            //                      font-size: 14px;
            //                  }
            //                }
            //                  table { margin: auto; }
            //                  td { padding: 0.5em; }
            //              </style>
            //              <script language='javascript'>
            //                  function api(method, path, payload, callback) {
            //                      var xhr = new XMLHttpRequest();
            //                      xhr.onreadystatechange = function () {
            //                          if (this.readyState == 4) {
            //                              callback(this.responseText);
            //                          }
            //                      };
            //                      xhr.open(method, document.location.protocol + '//' + document.location.host + '/' + path, true);
            //                      if (payload) {
            //                          xhr.setRequestHeader('Content-type', 'application/json');
            //                          xhr.send(JSON.stringify(payload));
            //                      }
            //                      else {
            //                          xhr.send();
            //                      }
            //                  }
            //                  function join(meetingUrl) {
            //                      api('POST', 'joinCall', { JoinURL: meetingUrl }, _ => { updateCalls(); });
            //                  }
            //                  function leave(legId) {
            //                      api('DELETE', 'calls/' + legId, null, _ => { window.setTimeout(updateCalls, 2000); });
            //                  }
            //                  function updateCalls() {
            //                      api('GET', 'calls', null, rsp => {
            //                          var htm = '<table>';
            //                          if (rsp) {
            //                              var calls = JSON.parse(rsp);
            //                              htm += '<tr><th></th><th>LegID</th><th>ScenarioID</th><th></th></tr>';
            //                              for (var i = 0; i < calls.length; i++) {
            //                                  var call = calls[i];
            //                                  htm += '<tr>';
            //                                  htm += '<td><button onclick=""leave(\'' + call.legId + '\')"">Leave</button>';
            //                                  htm += '<td>' + call.legId + '</td>';
            //                                  htm += '<td>' + call.scenarioId + '</td>';
            //                                  htm += '<td><a target=""_blank"" href=""' + call.logs + '"">Logs</a></td>';
            //                                  htm += '</tr>';
            //                              }
            //                          }
            //                          htm += '</table>';
            //                          document.getElementById('calls').innerHTML = htm;
            //                      });
            //                  }
            //              </script>
            //          </head>
            //          <body onload='updateCalls()'>
            //              <h1>Junta de Vecinos</h1>
            //              <input name='JoinURL' type='text' id='Url' />
            //              <button onclick='join(document.getElementById(""joinUrl"").value)'>Join Meeting</button>
            //              <hr />
            //              <h1>Lista de Llamadas</h1>
            //              <button onclick='updateCalls()'>Actualizar</button>
            //              <div id='calls' />
            //          </body>
            //      </html>";

            var html = string.Empty;

            return Content(html, "text/html", Encoding.UTF8); ;
        }
    }
}