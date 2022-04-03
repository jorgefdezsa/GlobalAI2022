import { combineReducers } from "redux";
import { httpReducer, HttpState } from "./http";

export interface StateReducer {
  http: HttpState;
}

export const state = combineReducers<StateReducer>({
  http: httpReducer,
});
