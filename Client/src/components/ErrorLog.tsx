import React from "react";
import { Container, Table } from "react-bootstrap";
import { ErrorRecord } from "../models/ErrorRecord";
import { FileUploadService } from "../services/FileUploadService";

interface State {
    errors: ErrorRecord[];
}

export default class ErrorLog extends React.Component<{}, State> {
    constructor(props: any) {
        super(props);
        this.state = {
            errors: []
        };
    }
    async componentDidMount() {
        const errors = await FileUploadService.GetErrors();
        this.setState({ errors });
    }
    render() {
        const { errors } = this.state;
        return (<Container>
            <Table striped bordered hover>
                <thead>
                    <tr>
                        <th>Date Created</th>
                        <th>File Name</th>
                        <th>Message</th>
                        <th>Line #</th>
                        <th>Column #</th>
                        <th>Record</th>
                    </tr>
                </thead>
                <tbody>
                    {errors.map(error => (
                        <tr>
                            <td>{error.dateCreated}</td>
                            <td>{error.fileName}</td>
                            <td>{error.errorMessage}</td>
                            <td>{error.lineNumber}</td>
                            <td>{error.columnNumber}</td>
                            <td>{error.line}</td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </Container>);
    }
}