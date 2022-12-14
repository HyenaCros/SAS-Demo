import React from "react";
import { Container, Nav, Navbar } from "react-bootstrap";
import { Outlet } from "react-router";
import { Link } from "react-router-dom";

export default class Root extends React.PureComponent {
  render() {
    return (<>
      <Navbar bg="dark" expand="lg" variant="dark">
        <Container>
          <Navbar.Brand as={Link} to="upload">SAS Demo</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <Nav.Link as={Link} to="upload">Upload</Nav.Link>
              <Nav.Link as={Link} to="errors">Error Log</Nav.Link>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <Container fluid className="pt-3 flex-grow-1 overflow-auto bg-light">
        <Outlet />
      </Container>
    </>);
  }
}
