import React from 'react';
import { exp } from 'react-native-reanimated';
import styled from 'styled-components/native';

export const Container = styled.SafeAreaView`
    background-color: #63C2D1;
    flex: 1;
    justify-content: center;
    align-items: center;
`;
export const InputArea = styled.View`
    margin-top: 15px;
    width: 100%;
    padding: 40px;
`;

export const CustumButton = styled.TouchableOpacity`
    height: 60px;
    background-color: #268596;
    border-radius: 30px;
    justify-content: center;
    align-items: center;
    margin-top: 10px;
`;
export const CustumButtonText = styled.Text`
    font-size: 18px;
    color: #FFF;
`;

export const SignMessageButton = styled.TouchableOpacity`
    flex-direction: row;
    justify-content: center;
    margin-top: 50px;
    margin-bottom: 20px;
`;
export const SignMessageButtonText = styled.Text`
    font-size: 16px;
    color: #268596;
`;
export const SignMessageButtonTextBold = styled.Text`
    font-size: 16px;
    color: #268596;
    font-weight: bold;
    margin-left: 5px;
`;
export const HeaderArea = styled.View`
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    
`;

export const HeaderTitle = styled.Text`
    margin-top: 40px;
    width: 250px;
    font-size:30px;
    font-weight: bold;
    color: #FFF;
`;
export const Scroller = styled.ScrollView`
    padding: 20px;
    margin-top: 20px;
`;