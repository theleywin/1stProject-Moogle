#!/bin/bash
clear

run(){
    echo "\e[45m                                                                                                                       \e[49m

   __ _                 _                  _                                  _            
  /__(_) ___  ___ _   _| |_ __ _ _ __   __| | ___     /\/\   ___   ___   __ _| | ___       
 /_\ | |/ _ \/ __| | | | __/ _     _ \ / _  |/ _ \   /    \ / _ \ / _ \ / _  | |/ _ \      
//__ | |  __/ (__| |_| | || (_| | | | | (_| | (_) | / /\/\ \ (_) | (_) | (_| | |  __/_ _ _ 
\__/_/ |\___|\___|\__,_|\__\__,_|_| |_|\__,_|\___/  \/    \/\___/ \___/ \__, |_|\___(_|_|_)
   |__/                                                                 |___/              

\e[45m                                                                                                                       \e[49m"
                                            
    cd ../
    dotnet watch run --project MoogleServer
}

report(){
    clear 
    echo "\e[45m                                                                                                                       \e[49m


   ___                      _ _                 _           __ _    _____        __                                 
  / __\___  _ __ ___  _ __ (_) | __ _ _ __   __| | ___     /__\ |   \_   \_ __  / _| ___  _ __ _ __ ___   ___       
 / /  / _ \|  _   _ \|  _ \| | |/ _  |  _ \ / _  |/ _ \   /_\ | |    / /\/  _ \| |_ / _ \|  __|  _   _ \ / _ \      
/ /__| (_) | | | | | | |_) | | | (_| | | | | (_| | (_) | //__ | | /\/ /_ | | | |  _| (_) | |  | | | | | |  __/_ _ _ 
\____/\___/|_| |_| |_|  __/|_|_|\____|_| |_|\____|\___/  \__/ |_| \____/ |_| |_|_|  \___/|_|  |_| |_| |_|\___(_|_|_)
                     |_|                                                                                            
                                                                               

\e[45m                                                                                                                       \e[49m"
                                            
    cd ../
    cd Informe
    pdflatex -synctex=1 -interaction=nonstopmode informe.tex
}

slides(){
    clear 
    echo "\e[45m                                                                                                                       \e[49m 

   ___                      _ _                 _            
  / __\___  _ __ ___  _ __ (_) | __ _ _ __   __| | ___       
 / /  / _ \| '_   _ \| '_ \| | |/ _  | '_ \ / _  |/ _ \      
/ /__| (_) | | | | | | |_) | | | (_| | | | | (_| | (_) | _ _ 
\____/\___/|_| |_| |_| .__/|_|_|\__,_|_| |_|\__,_|\___(_|_|_)
                     |_|                                     

                                            
\e[45m                                                                                                                       \e[49m"
    cd ../
    cd Presentacion
    pdflatex -synctex=1 -interaction=nonstopmode presentacion.tex
}

show_report(){
    cd ..
    cd Informe
    archivo="informe.pdf"

    if [ ! -e "$archivo" ]; then
        report
        xdg-open informe.pdf
    else
        xdg-open informe.pdf
    fi
}

show_report(){
    cd ..
    cd Presentacion
    archivo="presentacion.pdf"

    if [ ! -e "$archivo" ]; then
        slides
        xdg-open presentacion.pdf
    else
        xdg-open presentacion.pdf
    fi
}

clean(){
    cd ../
    cd Informe
    rm -v informe.aux
    rm -v informe.log
    rm -v informe.out
    rm -v informe.pdf
    rm -v informe.synctex.gz
    rm -v informe.toc

    cd ../
    cd Presentacion
    rm -v presentacion.aux
    rm -v presentacion.log
    rm -v presentacion.out
    rm -v presentacion.pdf
    rm -v presentacion.synctex.gz
    rm -v texput.log
}


if [ $1 = "run" ]; then
    clear
    run
fi

if [ $1 = "report" ]; then
    clear
    report
fi
if [ $1 = "slides" ]; then
    clear
    slides
fi
if [ $1 = "show_report" ]; then
    clear
    show_report
fi
if [ $1 = "show_slides" ]; then
    clear
    show_report
fi
if [ $1 = "clean" ]; then
    clear
    clean
fi


