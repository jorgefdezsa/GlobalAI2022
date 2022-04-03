import { JoinCall, JoinURLResponse } from '../models';
import { actionsDef } from '../actions/actionsDef';
import { JuntaDeVecinosAPI } from '../api/index';

// export const fetchJoinCallAction = (callUri: JoinCall) => (dispatch: any) => {
//     // debugger;
//     // dispatch(loading(true));
//     JuntaDeVecinosAPI.joinCall(callUri)
//         .then((response) => {
//             dispatch(fetchJoinCallCompleted(response));
//         })
//         .catch((error) => {
//         }).finally(() => {
//             // dispatch(loading(false));
//         });
// };

export const fetchJoinCallCompleted = (response: JoinURLResponse) => ({
    type: actionsDef.joinCall.FETCH_JOIN_CALL,
    payload: response,
    meta: {
        httpEnd: true
    }
});

export const loading = (loading: boolean) => ({
    type: actionsDef.joinCall.LOADING,
    payload: loading,
    meta: {
        httpEnd: true
    }
});