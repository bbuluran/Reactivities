import React, { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Grid } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { useParams } from "react-router-dom";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import ActivityDetailsHeader from "./ActivityDetailsHeader";
import ActivityDetailsInfo from "./ActivityDetailsInfo";
import ActivityDetailsChat from "./ActivityDetailsChat";
import ActivityDetailsSideBar from "./ActivityDetailsSideBar";

function ActivityDetails() {
    const {activityStore} = useStore();
    const {selectedActivity, loadActivity, loadingInitial} = activityStore;

    const {id} = useParams<{id: string}>();

    useEffect(() => {
        if (id) loadActivity(id);
    }, [id, loadActivity]);


    if (loadingInitial || !selectedActivity) return <LoadingComponent />;

    return (
        // <Card fluid>
        //     <Image src={`/assets/categoryImages/${selectedActivity?.category}.jpg`} />
        //     <Card.Content>
        //         <Card.Header>{selectedActivity?.title}</Card.Header>
        //         <Card.Meta>
        //             <span>{selectedActivity?.date}</span>
        //         </Card.Meta>
        //         <Card.Description>{selectedActivity?.description}</Card.Description>
        //     </Card.Content>
        //     <Card.Content extra>
        //         <Button.Group widths='2'>
        //             <Button as={Link} to={`/manage/${selectedActivity.id}`} basic color='blue' content='Edit'/>
        //             <Button as={Link} to='/activities' basic color='grey' content='Cancel'/>
        //         </Button.Group>
        //     </Card.Content>
        // </Card>

        <Grid>
            <Grid.Column width={10}>
                <ActivityDetailsHeader activity={selectedActivity}/>
                <ActivityDetailsInfo activity={selectedActivity} />
                <ActivityDetailsChat />
            </Grid.Column>
            <Grid.Column width={6}>
                <ActivityDetailsSideBar />
            </Grid.Column>
        </Grid>
    )
}

export default observer(ActivityDetails);
