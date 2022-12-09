import React from "react";
import { Button, Col, Container, Row, Table } from "react-bootstrap";
import { ErrorLogRecord } from "../types";
import { FileUploadService } from "../services/FileUploadService";
import { Subject, takeUntil } from "rxjs";
import ErrorLine from "./ErrorLine";

interface State {
  errors: ErrorLogRecord[];
}

export default class ErrorLog extends React.PureComponent<{}, State> {
  private _destroyed = new Subject<void>();
  constructor(props) {
    super(props);
    this.state = {
      errors: []
    };
  }

  async componentDidMount() {
    FileUploadService.ErrorLogs.pipe(takeUntil(this._destroyed)).subscribe(errorMap => {
      const errors = Object.keys(errorMap)
        .map(x => errorMap[x])
        .sort((a, b) => a.dateCreated < b.dateCreated 
          ? 1  : a.dateCreated > b.dateCreated
            ? -1 : a.lineNumber > b.lineNumber
              ? 1 : -1);
      this.setState({ errors })
    });
  }

  componentWillUnmount() {
    this._destroyed.next();
    this._destroyed.complete();
  }

  refresh = () => FileUploadService.UpdateErrorLogs();

  render() {
    const { errors } = this.state;
    return (
      <Row>
        <Col xs="12">
          <Button className="float-end mb-3" onClick={this.refresh}>Refresh</Button>
        </Col>
        <Col>
          <Table striped bordered hover>
            <thead className="text-bg-dark">
              <tr>
                <th>Date Created</th>
                <th>File Name</th>
                <th>Message</th>
                <th>Line #</th>
                <th>Column #</th>
              </tr>
            </thead>
            <tbody>
              {errors.map(error => (
                <ErrorLine error={error}>
                  <tr key={error.id}>
                    <td>{new Date(error.dateCreated).toLocaleString()}</td>
                    <td>{error.fileName}</td>
                    <td>{error.errorMessage}</td>
                    <td>{error.lineNumber}</td>
                    <td>{error.columnNumber}</td>
                  </tr>
                </ErrorLine>
              ))}
            </tbody>
          </Table>
        </Col>
      </Row>
    );
  }
}