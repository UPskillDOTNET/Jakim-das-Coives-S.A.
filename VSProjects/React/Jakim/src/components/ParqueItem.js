import React from 'react';
import styled from 'styled-components/native';

const Area = styled.TouchableOpacity``;
const InfoArea = styled.View``;
const UserName = styled.Text``;
const SeeProfileButton = styled.View``;
const SeeProfileButtonText = styled.Text``;


export default ({data}) => {
    return (
        <Area>
            <InfoArea>
                <UserName>{data.name}</UserName>
                <SeeProfileButton>
                    <SeeProfileButton>Efetuar Reserva</SeeProfileButton>
                </SeeProfileButton>
            </InfoArea>
        </Area>
    );
}
