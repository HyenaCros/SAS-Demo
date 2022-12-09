import React from "react";
import { OverlayTrigger, Popover } from "react-bootstrap";
import { ErrorRecord } from "../types";

interface Props {
  error: ErrorRecord;
  children: React.ReactElement;
}

export default class ErrorLine extends React.PureComponent<Props> {

  renderErrorLine = (line: string, errorColumn: number) => {
    if (errorColumn == null) {
      return line;
    }
    const columns: any[] = line.split('|');
    if (errorColumn > columns.length)
      return line;
    for (let i = 0; i < columns.length; i++) {
      const column = columns[i];
      columns[i] = <>
        {i == errorColumn
          ? <b>{column}</b>
          : column}
        {i < columns.length - 1 && <>|</>}
      </>
    }
    return columns;
  }

  render() {
    const { error } = this.props;
    return error.line ? 
      <OverlayTrigger trigger="hover" placement="top" delay={500} overlay={
        <Popover>
          <Popover.Header>Error Line</Popover.Header>
          <Popover.Body>
            {this.renderErrorLine(error.line, error.columnNumber)}
          </Popover.Body>
        </Popover>
      }>
        {this.props.children}
      </OverlayTrigger>
      : this.props.children;
  }
}