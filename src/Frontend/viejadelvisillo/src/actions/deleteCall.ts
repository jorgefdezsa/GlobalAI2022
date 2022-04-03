import { JoinURLResponse } from "../models";
import { actionsDef } from "../actions/actionsDef";
import { JuntaDeVecinosAPI } from "../api/index";

export const fetchDeleteCallAction = (callId: string) => (dispatch: any) => {
  dispatch(loading(true));
  JuntaDeVecinosAPI.deleteCall(callId)
    .then((response) => {
      dispatch(fetchDeleteCallCompleted(response));
    })
    .catch((error) => {})
    .finally(() => {
      dispatch(loading(false));
    });
};

export const fetchDeleteCallCompleted = (response: JoinURLResponse) => ({
  type: actionsDef.deleteCall.FETCH_DELETE_CALL,
  payload: response,
  meta: {
    httpEnd: true,
  },
});

export const loading = (loading: boolean) => ({
  type: actionsDef.deleteCall.LOADING,
  payload: loading,
  meta: {
    httpEnd: true,
  },
});
