import React from "react";
import { Button, Card, Container } from "react-bootstrap";
import CardHeader from "react-bootstrap/esm/CardHeader";
import { FileWatcherService } from "../services/FileWatcherService";

interface State {
  status: boolean;
  updating: boolean;
}

export default class FileWatcher extends React.PureComponent<{}, State> {
  constructor(props) {
    super(props);
    this.state = {
      status: false,
      updating: false
    }
  }
  async componentDidMount() {
    await this.updateStatus();
  }
  updateStatus = async () => {
    const status = await FileWatcherService.GetStatus();
    this.setState({ status, updating: false });
  }
  toggle = async () => {
    this.setState({ updating: true });
    if (this.state.status)
      await FileWatcherService.Stop();
    else
      await FileWatcherService.Start();
    await this.updateStatus();
  }
  render() {
    const { status, updating } = this.state;
    return (
      <Button className="float-start" onClick={this.toggle} disabled={updating}>{status ? 'Disable' : 'Enable'} File Watcher</Button>
    );
  }
}