import React from 'react';
import styled from 'styled-components/native';

export const Container = styled.SafeAreaView`
    background-color: #63C2D1;
    flex: 1;
    justify-content: center;
    align-items: center;    
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
export const InputArea = styled.View`
    margin-top: 15px;
    width: 100%;
    padding: 40px;
    position: absolute;
    bottom: 0;
`;
export const CustomButton = styled.TouchableOpacity`
    height: 60px;
    background-color: #268596;
    border-radius: 30px;
    justify-content: center;
    align-items: center;
    margin-top: 10px;
`;
export const CustomButtonText = styled.Text`
    font-size: 18px;
    color: #FFF;
`;
export const InpotArea = styled.View`
    margin-top: 15px;
    width: 100%;
    padding: 40px;
    position: absolute;
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
