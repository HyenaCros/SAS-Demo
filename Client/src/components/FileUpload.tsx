import React, { ChangeEvent, FormEvent } from "react";
import { Button, Card, Container, Form } from "react-bootstrap";
import CardHeader from "react-bootstrap/esm/CardHeader";
import { FileUploadService } from "../services/FileUploadService";

interface State {
  validated: boolean;
  file: File;
  type: string;
}

export default class FileUpload extends React.Component<{}, State> {
  constructor(props: any) {
    super(props);
    this.state = {
      validated: false,
      file: null,
      type: ""
    };
  }
  handleSumbit = async (event: FormEvent) => {
    event.preventDefault();
    event.stopPropagation();
    const form = event.currentTarget as HTMLFormElement;
    
    if (form.checkValidity() === false) {
      this.setState({ validated: true });
      return;
    }

    await FileUploadService.Upload(this.state.file, this.state.type);
    this.setState({ validated: false, file: null, type: "" });
    form.reset();
  }
  updateType = (e: ChangeEvent<HTMLSelectElement>) => {
    this.setState({ type: e.target.value, validated: false });
  }
  updateFile = (e: ChangeEvent<HTMLInputElement>) => {
    this.setState({ file: e.target.files[0], validated: false });
  }
  render() {
    const { validated, file, type } = this.state;
    return (<Container>
      <Form noValidate onSubmit={this.handleSumbit} validated={validated}>
        <Card>
          <CardHeader>File Upload</CardHeader>
          <Card.Body>
            <Form.Group className="mb-3">
              <Form.Label>File</Form.Label>
              <Form.Control type="file" required onChange={this.updateFile} />
              <Form.Control.Feedback/>
            </Form.Group>
            <Form.Group>
              <Form.Label>Type</Form.Label>
              <Form.Select required onChange={this.updateType} value={type}>
                <option value="" style={{display: 'none'}}>Select a type</option>
                <option>Medical</option>
                <option>Hospital</option>
                <option>Dental</option>
                <option>Prescription</option>
              </Form.Select>
              <Form.Control.Feedback/>
            </Form.Group>
          </Card.Body>
          <Card.Footer>
            <Button type="submit">Submit</Button>
          </Card.Footer>
        </Card>
      </Form>
    </Container>);
  }
}