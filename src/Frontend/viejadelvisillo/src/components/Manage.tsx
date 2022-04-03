import React, { useState, useEffect } from "react";
import styled from "styled-components";
import { ListFormat } from "typescript";
import { JuntaDeVecinosAPI } from "../api";
import { JoinURLResponse } from "../models";



export default function useJoinCall() {
  const [list, setList] = useState<JoinURLResponse>();
  const [query, setQuery] = useState("");

  const submmitCall = async (): Promise<void> => {
    let mounted = true;
    if (query.length > 0)
      JuntaDeVecinosAPI.getJoinCall(query).then((items) => {
        if (mounted) {
          setList(items);
        }
      });
  };



  function showTable() {
    if (list != undefined && list.callId != undefined)
      return (
        <TableStyles>
          <table>
            <tr>
              <th></th>
              <th>LegID</th>
              <th>ScenarioID</th>
              <th></th>
            </tr>
            <tr>
              <td>
                <a>
                  <img
                    src="https://urltostorageaccount/imgs/close.png"
                    alt=""
                  />
                </a>
              </td>
              <td>
                {list != undefined && list.callId != undefined
                  ? list.callId
                  : ""}
              </td>
              <td>
                {list != undefined && list.scenarioId != undefined
                  ? list.scenarioId
                  : ""}
              </td>
              <td>
                <a>
                  <img
                    src="https://urltostorageaccount/imgs/log-file.png"
                    alt=""
                  />
                </a>
              </td>
            </tr>
          </table>
        </TableStyles>
      );
  }

  return (
    <Container>
      <h1>Junta de Vecinos</h1>
      <input
        type="text"
        onChange={(event) => setQuery(event.target.value)}
        title="fff"
        placeholder=""
      />
      <Btn onClick={() => submmitCall()}>Conectar</Btn>
      {showTable()}
    </Container>
  );
}

const Container = styled.div`
  display: flex;
  align-items: center;
  flex-direction: column;

  h1 {
    padding: 20px 0;
    font-size: 16px;
    letter-spacing: 1.42px;
    text-transform: uppercase;
  }

  input {
    border-radius: 10px;
    width: 400px;
    height: 40px;
    color: white;
    background-color: #090b13;
    opacity: 1.5;
  }
`;

const Btn = styled.button`
  border-radius: 10px;
  border: 3px solid rgba(249, 249, 249, 0.1);
  box-shadow: rgb(0 0 0 / 69%) 0px 26px 30px -10px,
    rgb(0 0 0 / 73%) 0px 16px 10px -10px;
  transition: all 250ms cubic-bezier(0.25, 0.46, 0.45, 0.94) 0s;
  background-color: transparent;
  color: white;
  font-weight: 600;
  display: flex;
  padding: 15px;
  cursor: pointer;
  margin-top: 10px;

  &:hover {
    color: black;
    background-color: white;
  }
`;

const Banner = styled.img`
  border-radius: 50px;
  width: 400px;
  height: 50%;
  padding: 40px;
`;

const TableStyles = styled.div`
  padding: 3rem;

  table {
    border-spacing: 0;
    border: 1px solid black;
    border-top-right-radius: 20px;
    border-top-left-radius: 20px;
    overflow: hidden;

    tr {
      :last-child {
        td {
          border-bottom: 0;
        }
      }
    }

    th,
    td {
      margin: 0;
      padding: 0.5rem;
      font-size: 12px;
      letter-spacing: 1.42px;

      :last-child {
        border-right: 0;
      }
    }

    th {
      background: #090b13;
      text-transform: uppercase;
    }

    td {
      color: white;
      font-weight: 600;
    }

    a {
      color: white;
      justify-content: center;
      display: flex;
      cursor: pointer;
      background: transparent;

      img {
        height: 40px;
      }
    }
  }
`;
