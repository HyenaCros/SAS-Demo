import React from "react";
import { Button, Col, Container, Row, Table } from "react-bootstrap";
import { useNavigate, NavigateFunction } from "react-router-dom";
import { Subject, takeUntil } from "rxjs";
import { FileUploadService } from "../services/FileUploadService";
import { ClaimType, FileSource, FileUploadRecord, UploadStatus } from "../types";
import FileUpload from "./FileUpload";

interface Props {
  navigate: NavigateFunction
}

interface State {
  fileUploads: FileUploadRecord[];
}

class FileUploadView extends React.PureComponent<Props, State> {
  private _destroyed = new Subject<void>();

  constructor(props: Props) {
    super(props);
    this.state = {
      fileUploads: []
    };
  }

  componentDidMount() {
    FileUploadService.FileUploads.pipe(takeUntil(this._destroyed)).subscribe(fileUploadMap => {
      const fileUploads = Object.keys(fileUploadMap).map(x => fileUploadMap[x]);
      this.setState({ fileUploads });
    });
  }

  componentWillUnmount() {
    this._destroyed.next();
    this._destroyed.complete();
  }

  refresh = () => FileUploadService.UpdateFileUploads();

  render() {
    const { fileUploads } = this.state;
    const { navigate } = this.props;
    return (
      <Row>
        <Col xs="12">
          <Button className="float-end mb-3" onClick={this.refresh}>Refresh</Button>
        </Col>
        <Col sm="12" md="4">
          <FileUpload />
        </Col>
        <Col>
          <Table striped bordered hover>
            <thead className="text-bg-dark">
              <tr>
                <th>Name</th>
                <th>Date Uploaded</th>
                <th>Source</th>
                <th>Type</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              {fileUploads.map(fileUpload => (
                <tr key={fileUpload.id} className="pointer" onClick={() => navigate(`/detail/${fileUpload.id}`)}>
                  <td>{fileUpload.fileName}</td>
                  <td>{new Date(fileUpload.dateUploaded).toLocaleString()}</td>
                  <td>{FileSource[fileUpload.source]}</td>
                  <td>{ClaimType[fileUpload.type]}</td>
                  <td>{UploadStatus[fileUpload.status]}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </Col>
      </Row>
    );
  }
}

export default (props) => <FileUploadView navigate={useNavigate()}/>;