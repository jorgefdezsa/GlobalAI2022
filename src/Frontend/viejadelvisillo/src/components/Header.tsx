import React from "react";
import styled from "styled-components";

function Header() {
  return (
    <Nav>
      <Logo src="https://urltostorageaccount/imgs/parrita_logo.png" />
      <NavMenu>
        <a href="/">
          <img
            src="https://urltostorageaccount/imgs/home-icon.svg"
            alt=""
          />
          <span>INICIO</span>
        </a>
        <a href="/process">
          <img
            src="https://urltostorageaccount/imgs/original-icon.svg"
            alt=""
          />
          <span>EXTRAER PALABRAS</span>
        </a>
      </NavMenu>
      <UserImg
        src="https://yt3.ggpht.com/yti/APfAmoGU2phakWR8ro_JSVAdcG6cTpFOeON_hxsEYVOgV_0=s88-c-k-c0x00ffffff-no-rj-mo"
        alt=""
      />
    </Nav>
  );
}

export default Header;

const Nav = styled.nav`
  height: 70px;
  background: #090b13;
  display: flex;
  align-items: center;
  padding: 0 36px;
`;

const Logo = styled.img`
  width: 80px;
`;

const NavMenu = styled.div`
  display: flex;
  flex: 1;
  margin-left: 20px;
  align-items: center;

  a {
    display: flex;
    align-items: center;
    padding: 0 12px;
    cursor: pointer;

    img {
      height: 20px;
    }

    span {
      font-size: 12px;
      letter-spacing: 1.42px;
      position: relative;

      &:after {
        content: "";
        height: 2px;
        background: white;
        position: absolute;
        left: 0;
        right: 0;
        bottom: -6px;
        opacity: 0;
        transform-origin: left center;
        transition: all 250ms cubic-bezier(0.25, 0.46, 0.45, 0.94) 0s;
        transform: scaleX(0);
      }
    }

    &:hover {
      span:after {
        transform: scaleX(1);
        opacity: 1;
      }
    }
  }
`;

const UserImg = styled.img`
  width: 48px;
  height: 48px;
  border-radius: 50%;
  cursor: pointer;
`;
