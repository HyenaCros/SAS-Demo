import React from "react";
import { Button, Card, Container } from "react-bootstrap";
import CardHeader from "react-bootstrap/esm/CardHeader";
import { FileWatcherService } from "../services/FileWatcherService";

interface State {
  status: boolean;
}

export default class FileWatcher extends React.Component<{}, State> {
  constructor(props: any) {
    super(props);
    this.state = {
      status: false
    }
  }
  async componentDidMount() {
    await this.updateStatus();
  }
  updateStatus = async () => {
    const status = await FileWatcherService.GetStatus();
    this.setState({ status });
  }
  enable = async () => {
    await FileWatcherService.Start();
    await this.updateStatus();
  }
  disable = async () => {
    await FileWatcherService.Stop();
    await this.updateStatus();
  }
  render() {
    const { status } = this.state;
    return (<Container>
      <Card>
        <CardHeader>File Watcher Status</CardHeader>
        <Card.Body>
          {status.toString()}
        </Card.Body>
        <Card.Footer>
          <Button onClick={this.enable}>Enable</Button>
          <Button onClick={this.disable}>Disable</Button>
        </Card.Footer>
      </Card>
    </Container>);
  }
}