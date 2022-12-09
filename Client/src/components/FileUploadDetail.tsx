import React from "react";
import { Button, Col, OverlayTrigger, Popover, Row, Stack, Table } from "react-bootstrap";
import { useParams, useNavigate, NavigateFunction } from "react-router-dom";
import { FileUploadService } from "../services/FileUploadService";
import { ClaimType, ErrorRecord, FileClaimRecord, FileUploadRecord } from "../types";
import ErrorLine from "./ErrorLine";

interface Props {
  params: { id: string }
  navigate: NavigateFunction
}
interface State {
  fileUpload: FileUploadRecord;
}
class FileUploadDetail extends React.PureComponent<Props, State> {
  constructor(props: Props) {
    super(props);
    this.state = {
      fileUpload: null
    }
  }

  componentDidMount() {
    this.getFileUpload();
  }

  componentDidUpdate(prevProps: Props) {
    if (prevProps.params.id != this.props.params.id)
      this.getFileUpload();
  }

  async getFileUpload() {
    const fileUpload = await FileUploadService.GetFileUpload(this.props.params.id);
    if (fileUpload) {
      this.setState({ fileUpload });
      return;
    }
    this.props.navigate('/');
  }
  
  renderClaimsTable = (claims: FileClaimRecord[], type: ClaimType) => (
    <div>
      <h4>{ClaimType[type]} Claims</h4>
      <Table striped bordered hover className="mb-3">
      <thead className="text-bg-dark">
        <tr>
          <th>Claim Number</th>
          <th>Claim Date</th>
          <th>Line #</th>
          <th>Patient ID</th>
          <th>Provider ID</th>
          <th>Amount</th>
          {type != ClaimType.Prescription
            ? <>
              <th>Procedure Date</th>
              <th>Procedure Code</th>
            </>
            : <th>Drug ID</th>}
        </tr>
      </thead>
      <tbody>
        {claims.map(claim => (
          <tr key={claim.id} className="pointer">
            <td>{claim.claimNumber}</td>
            <td>{new Date(claim.claimDate).toLocaleString()}</td>
            <td>{claim.claimLineNumber}</td>
            <td>{claim.patientId}</td>
            <td>{claim.providerId}</td>
            <td>{claim.claimAmount}</td>
            {type != ClaimType.Prescription
              ? <>
                <td>{new Date(claim.procedureDate).toLocaleString()}</td>
                <td>{claim.procedureCode}</td>
              </>
              : <td>{claim.drugId}</td>
            }
          </tr>
        ))}
      </tbody>
    </Table>
    </div>
  )

  renderErrorTable = (errors: ErrorRecord[]) => (
    <div>
      <h4>Errors</h4>
      <Table striped bordered hover>
        <thead className="text-bg-dark">
          <tr>
            <th>Message</th>
            <th>Line #</th>
            <th>Column #</th>
          </tr>
        </thead>
        <tbody>
          {errors.map(error => (
            <ErrorLine error={error}>
              <tr key={error.id}>
                <td>{error.errorMessage}</td>
                <td>{error.lineNumber}</td>
                <td>{error.columnNumber}</td>
              </tr>
            </ErrorLine>
          ))}
        </tbody>
      </Table>
    </div>
  )

  render() {
    const { fileUpload } = this.state;
    if (!fileUpload)
      return;

    return (
      <Stack gap={3}>
        <Stack gap={3} direction="horizontal">
          <Button onClick={() => this.props.navigate(-1)}>Back</Button>
          <h3>{fileUpload.fileName}</h3>
        </Stack>
        {fileUpload.medicalClaims.length > 0 && this.renderClaimsTable(fileUpload.medicalClaims, ClaimType.Medical)}
        {fileUpload.hospitalClaims.length > 0 && this.renderClaimsTable(fileUpload.hospitalClaims, ClaimType.Hospital)}
        {fileUpload.dentalClaims.length > 0 && this.renderClaimsTable(fileUpload.dentalClaims, ClaimType.Dental)}
        {fileUpload.prescriptionClaims.length > 0 && this.renderClaimsTable(fileUpload.prescriptionClaims, ClaimType.Prescription)}
        {fileUpload.errors.length > 0 && this.renderErrorTable(fileUpload.errors)}
      </Stack>
    );
  }
}

export default (props) => (<FileUploadDetail {...props} params={useParams()} navigate={useNavigate()} />);