


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Collections;
using ReactiveUI;



namespace Emulator_PPU.Models
{
    #region enum
    public enum ppu_status_bit
    {
        BLOCK_MASTER = 0,    // блок назначен ведущим
        BLOCK_SLAVE = 1,    // блок назначен ведомым
        CONNECTION_ESTABLISHED = 2,    // связь между блоками установлена
        BLOCK_CONFIGURED = 3,    // блок сконфигурирован
        BLOCK_POWER_ON = 4,    // питание включено
        BLOCK_PULSES_ON = 5,    // импульсы включены
        USB_ESTABLISHED = 6,    // юсб установлено
        FAILURE = 7,    // сбой
        EXTERNAL_BLOCKING = 8,    // внешняя блокировка
        POWER_SUPPLY_FAILURE = 9,    // сбой источника питания
        NETWORK_CONFIG_ERR = 10,   // ошибка конфигурации сети
        BLOCK_CONFIG_ERR = 11,   // ошибка конфигурации блока
        CHANNEL_CONFIG_ERR = 12,   // ошибка конфигурации канала
        OVERHEAT_DISCHARGE_CIRCUIT = 13,   // перегрев разрядной цепи
    }

    public enum channel_status_bit
    {
        CHANNEL_ALLOWED = 0,    // канал разрешен
        CHANNEL_CONFIGURED = 1, // канал сконфигурирован
        VOLT_REGULATOR_ON = 2,  // регулятор напряжения включен
        PULSES_ON = 3,    // импульсы включены
        EXCEED_PULSE_CURRENT = 4,    // превышение импульсного тока
        EXCEED_VOLT_TRANSISTOR = 5,    // превышение напряжения на транзисторе
        TRANSISTOR_OVERHEAT = 6,    // перегрев транзистора
        CHANNEL_MISSED = 7,    // канал отсутствует
    }
    #endregion

    public class Flag : ReactiveObject
    {
        private bool state;
        private string name;
        public Flag(string name, bool state = false)
        {
            this.name = name;
            this.state = state;
        }
    }

    public class Channel : ReactiveObject 
    {
        public Channel(int index, bool channel_missed = false)
        {
            this.index = index;
            if(channel_missed)
            {
                CHANNEL_ALLOWED = false;
                CHANNEL_CONFIGURED = false;
                CHANNEL_MISSED = true;
            }
        }

        #region private bool
        private int _index;
        private bool _CHANNEL_ALLOWED = true;    // канал разрешен
        private bool _CHANNEL_CONFIGURED = true; // канал сконфигурирован
        private bool _VOLT_REGULATOR_ON;  // регулятор напряжения включен
        private bool _PULSES_ON;    // импульсы включены
        private bool _EXCEED_PULSE_CURRENT;    // превышение импульсного тока
        private bool _EXCEED_VOLT_TRANSISTOR;    // превышение напряжения на транзисторе
        private bool _TRANSISTOR_OVERHEAT;    // перегрев транзистора
        private bool _CHANNEL_MISSED;    // канал отсутствует
        #endregion

        #region property
        public int index
        {
            get => _index;
            set => this.RaiseAndSetIfChanged(ref _index, value);
        }
        public bool CHANNEL_ALLOWED    // канал разрешен
        {
            get => _CHANNEL_ALLOWED;
            set => this.RaiseAndSetIfChanged(ref _CHANNEL_ALLOWED, value);
        }
        public bool CHANNEL_CONFIGURED    // канал сконфигурирован
        {
            get => _CHANNEL_CONFIGURED;
            set => this.RaiseAndSetIfChanged(ref _CHANNEL_CONFIGURED, value);
        }
        public bool VOLT_REGULATOR_ON    // регулятор напряжения включен
        {
            get => _VOLT_REGULATOR_ON;
            set => this.RaiseAndSetIfChanged(ref _VOLT_REGULATOR_ON, value);
        }
        public bool PULSES_ON    // импульсы включены
        {
            get => _PULSES_ON;
            set => this.RaiseAndSetIfChanged(ref _PULSES_ON, value);
        }
        public bool EXCEED_PULSE_CURRENT     // превышение импульсного тока
        {
            get => _EXCEED_PULSE_CURRENT;
            set => this.RaiseAndSetIfChanged(ref _EXCEED_PULSE_CURRENT, value);
        }
        public bool EXCEED_VOLT_TRANSISTOR     // превышение напряжения на транзисторе
        {
            get => _EXCEED_VOLT_TRANSISTOR;
            set => this.RaiseAndSetIfChanged(ref _EXCEED_VOLT_TRANSISTOR, value);
        }
        public bool TRANSISTOR_OVERHEAT     // перегрев транзистора
        {
            get => _TRANSISTOR_OVERHEAT;
            set => this.RaiseAndSetIfChanged(ref _TRANSISTOR_OVERHEAT, value);
        }
        public bool CHANNEL_MISSED     // канал отсутствует
        {
            get => _CHANNEL_MISSED;
            set => this.RaiseAndSetIfChanged(ref _CHANNEL_MISSED, value);
        }
        #endregion
    }


    public class PPU : ReactiveObject
    {
        public PPU()
        {
            for (int i = 0; i <6; i++)
            {
                if(i == 5)
                {
                    channels.Add(new(i, true));
                }
                else  channels.Add(new(i));
            }
        }

        public short PPU_getStatus()
        {
            short status = 0;

            // общий статус
            if (BLOCK_MASTER)
                status |=  1 << (int)ppu_status_bit.BLOCK_MASTER;
            if (BLOCK_SLAVE)
                status |= 1 << (int)ppu_status_bit.BLOCK_SLAVE;
            if (CONNECTION_ESTABLISHED)
                status |= 1 << (int)ppu_status_bit.CONNECTION_ESTABLISHED;
            if (BLOCK_CONFIGURED)
                status |= 1 << (int)ppu_status_bit.BLOCK_CONFIGURED;
            if (BLOCK_POWER_ON)
                status |= 1 << (int)ppu_status_bit.BLOCK_POWER_ON;
            if (BLOCK_PULSES_ON)
                status |= 1 << (int)ppu_status_bit.BLOCK_PULSES_ON;
            if (USB_ESTABLISHED)
                status |= 1 << (int)ppu_status_bit.USB_ESTABLISHED;
            if (FAILURE)
                status |= 1 << (int)ppu_status_bit.FAILURE;
            if (EXTERNAL_BLOCKING)
                status |= 1 << (int)ppu_status_bit.EXTERNAL_BLOCKING;
            if (POWER_SUPPLY_FAILURE)
                status |= 1 << (int)ppu_status_bit.POWER_SUPPLY_FAILURE;
            if (NETWORK_CONFIG_ERR)
                status |= 1 << (int)ppu_status_bit.NETWORK_CONFIG_ERR;
            if (BLOCK_CONFIG_ERR)
                status |= 1 << (int)ppu_status_bit.BLOCK_CONFIG_ERR;
            if (CHANNEL_CONFIG_ERR)
                status |= 1 << (int)ppu_status_bit.CHANNEL_CONFIG_ERR;
            if (OVERHEAT_DISCHARGE_CIRCUIT)
                status |= 1 << (int)ppu_status_bit.OVERHEAT_DISCHARGE_CIRCUIT;

            return status;
        }

        public byte Channel_getStatus(int index)
        {
            byte status = 0;
            if (channels[index].CHANNEL_ALLOWED)
                status |= 1 << (int)channel_status_bit.CHANNEL_ALLOWED;
            if (channels[index].CHANNEL_CONFIGURED)
                status |= 1 << (int)channel_status_bit.CHANNEL_CONFIGURED;
            if (channels[index].VOLT_REGULATOR_ON)
                status |= 1 << (int)channel_status_bit.VOLT_REGULATOR_ON;
            if (channels[index].PULSES_ON)
                status |= 1 << (int)channel_status_bit.PULSES_ON;
            if (channels[index].EXCEED_PULSE_CURRENT)
                status |= 1 << (int)channel_status_bit.EXCEED_PULSE_CURRENT;
            if (channels[index].EXCEED_VOLT_TRANSISTOR)
                status |= 1 << (int)channel_status_bit.EXCEED_VOLT_TRANSISTOR;
            if (channels[index].TRANSISTOR_OVERHEAT)
                status |= 1 << (int)channel_status_bit.TRANSISTOR_OVERHEAT;
            if (channels[index].CHANNEL_MISSED)
                status |= 1 << (int)channel_status_bit.CHANNEL_MISSED;

            return status;
        }


        #region private
        private AvaloniaList<Channel>? _channels = new();
        private bool _BLOCK_MASTER;              // блок назначен ведущим
        private bool _BLOCK_SLAVE;               // блок назначен ведомым
        private bool _CONNECTION_ESTABLISHED;    // связь между блоками установлена
        private bool _BLOCK_CONFIGURED;          // блок сконфигурирован
        private bool _BLOCK_POWER_ON;            // питание включено
        private bool _BLOCK_PULSES_ON;           // импульсы включены
        private bool _USB_ESTABLISHED;           // юсб установлено
        private bool _FAILURE;                   // сбой
        private bool _EXTERNAL_BLOCKING;         // внешняя блокировка
        private bool _POWER_SUPPLY_FAILURE;      // сбой источника питания
        private bool _NETWORK_CONFIG_ERR;        // ошибка конфигурации сети
        private bool _BLOCK_CONFIG_ERR;          // ошибка конфигурации блока
        private bool _CHANNEL_CONFIG_ERR;        // ошибка конфигурации канала
        private bool _OVERHEAT_DISCHARGE_CIRCUIT;// перегрев разрядной цепи
        #endregion

        #region properties
        public AvaloniaList<Channel>? channels
        {
            get => _channels;
            set => this.RaiseAndSetIfChanged(ref _channels, value);
        }
        public bool BLOCK_MASTER     // блок назначен ведущим
        {
            get => _BLOCK_MASTER;
            set => this.RaiseAndSetIfChanged(ref _BLOCK_MASTER, value);
        }
        public bool BLOCK_SLAVE     // блок назначен ведомым
        {
            get => _BLOCK_SLAVE;
            set => this.RaiseAndSetIfChanged(ref _BLOCK_SLAVE, value);
        }
        public bool CONNECTION_ESTABLISHED     // связь между блоками установлена
        {
            get => _CONNECTION_ESTABLISHED;
            set => this.RaiseAndSetIfChanged(ref _CONNECTION_ESTABLISHED, value);
        }
        public bool BLOCK_CONFIGURED     // блок сконфигурирован
        {
            get => _BLOCK_CONFIGURED;
            set => this.RaiseAndSetIfChanged(ref _BLOCK_CONFIGURED, value);
        }
        public bool BLOCK_POWER_ON     // питание включено
        {
            get => _BLOCK_POWER_ON;
            set => this.RaiseAndSetIfChanged(ref _BLOCK_POWER_ON, value);
        }
        public bool BLOCK_PULSES_ON     // импульсы включены
        {
            get => _BLOCK_PULSES_ON;
            set => this.RaiseAndSetIfChanged(ref _BLOCK_PULSES_ON, value);
        }
        public bool USB_ESTABLISHED     // юсб установлено
        {
            get => _USB_ESTABLISHED;
            set => this.RaiseAndSetIfChanged(ref _USB_ESTABLISHED, value);
        }
        public bool FAILURE     // сбой
        {
            get => _FAILURE;
            set => this.RaiseAndSetIfChanged(ref _FAILURE, value);
        }
        public bool EXTERNAL_BLOCKING       // внешняя блокировка
        {
            get => _EXTERNAL_BLOCKING;  
            set => this.RaiseAndSetIfChanged(ref _EXTERNAL_BLOCKING, value);
        }
        public bool POWER_SUPPLY_FAILURE     // сбой источника питания
        {
            get => _POWER_SUPPLY_FAILURE;
            set => this.RaiseAndSetIfChanged(ref _POWER_SUPPLY_FAILURE, value);
        }
        public bool NETWORK_CONFIG_ERR     // ошибка конфигурации сети
        {
            get => _NETWORK_CONFIG_ERR;
            set => this.RaiseAndSetIfChanged(ref _NETWORK_CONFIG_ERR, value);
        }
        public bool BLOCK_CONFIG_ERR     // ошибка конфигурации блока
        {
            get => _BLOCK_CONFIG_ERR;
            set => this.RaiseAndSetIfChanged(ref _BLOCK_CONFIG_ERR, value);
        }
        public bool CHANNEL_CONFIG_ERR     // ошибка конфигурации канала
        {
            get => _CHANNEL_CONFIG_ERR;
            set => this.RaiseAndSetIfChanged(ref _CHANNEL_CONFIG_ERR, value);
        }
        public bool OVERHEAT_DISCHARGE_CIRCUIT     // перегрев разрядной цепи
        {
            get => _OVERHEAT_DISCHARGE_CIRCUIT;
            set => this.RaiseAndSetIfChanged(ref _OVERHEAT_DISCHARGE_CIRCUIT, value);
        }
        #endregion

    }
}
