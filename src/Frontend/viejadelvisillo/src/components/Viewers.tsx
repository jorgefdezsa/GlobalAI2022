import React, { useState, useEffect } from "react";
import styled from "styled-components";
import { JuntaDeVecinosAPI } from "../api";

export default function Viewers() {
  const [words, setWords] = useState<string[]>();
  const [query, setQuery] = useState("");

  const submmitCall = async (): Promise<void> => {
    let mounted = true;
    if (query.length > 0)
      JuntaDeVecinosAPI.processAudio(query).then((items) => {
        if (mounted) {
          setWords(items);
        }
      });
  };

  function ShowTable() {
    if (words != undefined)
    return(
        <Container>
          {
              words.map((name: string, index: number) => {
                  return (<Wrap><p>{name}</p></Wrap>)
            })
           }
        </Container>
    )
    else{
      return <Container></Container>;
    }
  }

  return (
    <Container1>
      <h1>Junta de Vecinos</h1>
      <h3>Introduzca nombre de sesion para procesar... (Demo1.wav, Demo2.wav)</h3>
      <input
        type="text"
        onChange={(event) => setQuery(event.target.value)}
        title="fff"
        placeholder=""
      />
      <Btn onClick={() => submmitCall()}>Procesar</Btn>
      {ShowTable()}
    </Container1>
  )
}

const Container1 = styled.div`
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

const Container = styled.div`
  margin-top: 30px;
  display: grid;
  padding: 30px 0px 26px;
  grid-gap: 25px;
  grid-template-columns: repeat(7, minmax(0, 1fr));
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

const Wrap = styled.div`
  border-radius: 10px;
  border: 3px solid rgba(249, 249, 249, 0.1);
  box-shadow: rgb(0 0 0 / 69%) 0px 26px 30px -10px,
    rgb(0 0 0 / 73%) 0px 16px 10px -10px;
  transition: all 250ms cubic-bezier(0.25, 0.46, 0.45, 0.94) 0s;

  p {
    font-family: "Licorice", cursive;
    object-fit: cover;
    font-size: 30px;
    text-align: center;
  }

  img {
    width: 100%;
    height: 100%;
    object-fit: cover;
  }

  &:hover {
    box-shadow: rgb(0 0 0 / 80%) 0px 40px 58px -16px,
      rgb(0 0 0 / 72%) 0px 30px 22px -10px;
    transform: scale(1.05);
    background-color: white;
    color: black;
    border-color: rgba(249, 249, 249, 0.8);
  }
`;
