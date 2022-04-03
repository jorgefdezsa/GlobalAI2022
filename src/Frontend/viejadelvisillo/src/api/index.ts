import { get, post, deleteAsync } from "./fetch-wrapper";
import * as fetch from "isomorphic-fetch";
import { HttpResponse } from "./http-response";
import { JoinCall, JoinURLResponse, ProcessAudio } from "../models";

const baseURL = process.env.REACT_APP_API;

const getJoinCall = async (join: string): Promise<JoinURLResponse> => {
  let callJoinURL: JoinCall = { JoinURL: join, DisplayName: "" };

  return post<JoinURLResponse>(`${baseURL}/joinCall`, callJoinURL).then(
    async (response) => await getJoinCallResponseDetails(response)
  );
};

const processAudio = async (fileName: string): Promise<string[]> => {
  let callProcessURL: ProcessAudio = { FileName: fileName };

  return post<string[]>(`${baseURL}/api/audio/process`, callProcessURL).then(
    async (response) => await getJoinCallResponseDetails(response)
  );
};


const deleteCall = async (callId: string): Promise<JoinURLResponse> => {
  return deleteAsync(`${baseURL}/calls/callLegId=${callId}`).then(
    async (response) => await getJoinCallResponseDetails(response)
  );
};

const getJoinCallResponseDetails = async (response: HttpResponse<any>) => {
  if (response.status < 203 && response.parsedBody) {
    return await response.parsedBody;
  } else {
    throw new Error(response.parsedBody ? response.parsedBody : "");
  }
};

const getResponseDetails = async (response: HttpResponse<any>) => {
  if (response.status < 203 && response.parsedBody) {
    return await response.parsedBody;
  } else {
    throw new Error(response.parsedBody ? response.parsedBody : "");
  }
};

export const JuntaDeVecinosAPI = {
  getJoinCall,
  deleteCall,
  processAudio
};
