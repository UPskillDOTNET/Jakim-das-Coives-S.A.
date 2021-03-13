import React, { useEffect, useState } from 'react';
import {FlatList, Text, AsyncStorage, Alert } from 'react-native';
import { useNavigation } from '@react-navigation/native';
import {
    Container,
    InputArea,
    InpotArea,
    CustumButton,
    CustumButtonText,
    CustomButton,
    CustomButtonText,
    HeaderArea,
    HeaderTitle,
    Scroller
} from './styles';
import SignInput from '../../components/SignInput';
import Api from '../../Api';
import EmailIcon from '../../assets/lock.svg';

export default () => {
    const navigation = useNavigation();
    const [nif, setNif] = useState('');


    //consult Saldo
    const handleClick = async () => {
        //navigation.navigate('Saldo');
            let json = await Api.getTransacao(nif);
                Alert.alert(
                    "As suas Transações: ",
                    JSON.stringify(json),
                    [
                      {
                        text: "Cancel",
                        onPress: () => console.log("Cancel Pressed"),
                        style: "cancel"
                      },
                      { text: "OK", onPress: () => console.log("OK Pressed") }
                    ],
                    { cancelable: false }
                  );
            }



    return (
        <Container>

            <Scroller>
                    <HeaderArea>
                        <HeaderTitle numberOfLines={2}>Consulte as suas Transações!</HeaderTitle>
                    </HeaderArea>
            </Scroller>

            <InpotArea>
                <CustomButton onPress={handleClick}>
                    <CustomButtonText>CONSULTAR</CustomButtonText>
                </CustomButton>
            </InpotArea>
        </Container>
    );
}